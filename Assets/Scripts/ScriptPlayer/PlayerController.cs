using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private AudioManager audioManager;
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Audio Movimiento")]
    [SerializeField] private float tiempoEntrePasos = 0.35f;
    private bool puedeSonarPaso = true;

    [Header("Movimiento")]
    [SerializeField] private float velocidad = 5f;
    [SerializeField] private string floorTag = "suelo";

    private float lastHorizontalDirection = 1f;
    private Bounds floorBounds;
    private bool floorDetectado = false;
    private Collider2D playerCollider;

    [Header("Sistema de Ataque")]
    [SerializeField] private bool debugMode = false;
    private readonly List<Ataque> ataquesDisponibles = new List<Ataque>();

    public bool IsDead { get; private set; } = false;
    public bool isAlive = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();

        audioManager = GameObject.FindGameObjectWithTag("Audio")
                      .GetComponent<AudioManager>();
    }

    private void Start()
    {
        InicializarAtaques();
        DetectarFloor();
    }

    private void DetectarFloor()
    {
        GameObject floor = GameObject.FindGameObjectWithTag(floorTag);
        if (floor != null)
        {
            Collider2D floorCollider = floor.GetComponent<Collider2D>();
            if (floorCollider != null)
            {
                floorBounds = floorCollider.bounds;
                floorDetectado = true;
            }
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f || IsDead)
            return;

        Movimiento();
        ProcesarAtaques();
    }

    private void InicializarAtaques()
    {
        ataquesDisponibles.Clear();
        ataquesDisponibles.AddRange(GetComponents<Ataque>());
    }

    private void ProcesarAtaques()
    {
        foreach (var ataque in ataquesDisponibles)
        {
            if (ataque == null) continue;

            if (Input.GetKeyDown(ataque.TeclaAtaque) && !ataque.EstaEnCooldown())
            {
                ataque.EjecutarAtaque();
                break;
            }
        }
    }

    public void DesbloquearAtaque(System.Type tipo)
    {
        if (!TryGetComponent(tipo, out Component existente))
        {
            gameObject.AddComponent(tipo);
            InicializarAtaques();
        }
    }

    public Vector2 GetLastMovementDirection() =>
        new Vector2(lastHorizontalDirection, 0f);

    private void Movimiento()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 direccion = new Vector2(horizontal, vertical);
        float magnitud = Mathf.Sqrt(direccion.x * direccion.x + direccion.y * direccion.y);

        Vector2 movimiento;
        if (magnitud > 0f)
        {
            movimiento = (direccion / magnitud) * velocidad * Time.deltaTime;
        }
        else
        {
            movimiento = Vector2.zero;
        }

        Vector2 nuevaPosicion = (Vector2)transform.position + movimiento;

        if (floorDetectado && playerCollider != null)
        {
            float halfWidth = playerCollider.bounds.extents.x;
            float halfHeight = playerCollider.bounds.extents.y;

            float minX = floorBounds.min.x + halfWidth;
            float maxX = floorBounds.max.x - halfWidth;
            float minY = floorBounds.min.y + halfHeight;
            float maxY = floorBounds.max.y - halfHeight;

            nuevaPosicion.x = Mathf.Clamp(nuevaPosicion.x, minX, maxX);
            nuevaPosicion.y = Mathf.Clamp(nuevaPosicion.y, minY, maxY);
        }

        transform.position = nuevaPosicion;

        animator.SetFloat("Horizontal", Mathf.Abs(horizontal));

        if (horizontal != 0)
        {
            lastHorizontalDirection = Mathf.Sign(horizontal);
            transform.localScale = new Vector3(lastHorizontalDirection, 1, 1);
        }

        if (Mathf.Abs(horizontal) > 0.1f && puedeSonarPaso)
            StartCoroutine(ReproducirPaso());
    }

    private IEnumerator ReproducirPaso()
    {
        puedeSonarPaso = false;
        audioManager.PlaySFX(audioManager.caminar);
        yield return new WaitForSeconds(tiempoEntrePasos);
        puedeSonarPaso = true;
    }

    private void OnDrawGizmos()
    {
        if (floorDetectado && debugMode)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(floorBounds.center, floorBounds.size);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.3f);
        }
    }
}