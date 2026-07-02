using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // DEPENDENCIAS
    private AudioManager audioManager;
    private Rigidbody2D rb;
    private Animator animator;
    // AUDIO
    [Header("Audio Movimiento")]
    [SerializeField] private float tiempoEntrePasos = 0.35f;
    private bool puedeSonarPaso = true;

    [Header("Movimiento")]
    [SerializeField] private float velocidad = 5f;

    private bool estaEnSuelo = false;
    private float lastHorizontalDirection = 1f;

    // ATAQUES
    [Header("Sistema de Ataque")]
    [SerializeField] private bool debugMode = false;
    private readonly List<Ataque> ataquesDisponibles = new List<Ataque>();


    // SISTEMA MUERTE
    public bool IsDead { get; private set; } = false;
    public bool isAlive = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        audioManager = GameObject.FindGameObjectWithTag("Audio")
                      .GetComponent<AudioManager>();
    }

    private void Start()
    {
        InicializarAtaques();
    }

    private void Update()
    {
        if (Time.timeScale == 0f || IsDead)
            return;

        Movimiento();
        ProcesarAtaques();
    }

    // ATAQUES
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


    // MOVIMIENTO
    private void Movimiento()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 direccion = new Vector2(horizontal, vertical);

        float magnitud = Mathf.Sqrt(direccion.x * direccion.x + direccion.y * direccion.y);

        Vector2 movimiento;
        if (magnitud > 0f)
        {

            movimiento = (direccion / magnitud) * velocidad;
        }
        else
        {
            movimiento = Vector2.zero;
        }

        if (estaEnSuelo)
        {
            rb.linearVelocity = movimiento;

            animator.SetFloat("Horizontal", Mathf.Abs(horizontal));

            if (horizontal != 0)
            {
                lastHorizontalDirection = Mathf.Sign(horizontal);
                transform.localScale = new Vector3(lastHorizontalDirection, 1, 1);
            }

            if (Mathf.Abs(horizontal) > 0.1f && puedeSonarPaso)
                StartCoroutine(ReproducirPaso());
        }
    }

    // SUELO (trigger)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("suelo"))
        {
            estaEnSuelo = true;
            rb.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("suelo"))
        {
            estaEnSuelo = false;
            rb.gravityScale = 1;
        }
    }

    // UTILIDAD
    private IEnumerator ReproducirPaso()
    {
        puedeSonarPaso = false;
        audioManager.PlaySFX(audioManager.caminar);
        yield return new WaitForSeconds(tiempoEntrePasos);
        puedeSonarPaso = true;
    }
}