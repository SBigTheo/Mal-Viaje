using UnityEngine;

public class EnemyPareja : MonoBehaviour
{
    //cCONFIGURACION
    [Header("Movimiento")]
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private bool flipToFacePlayer = true;
    [SerializeField] private float sueloNivel = -2.5f;

    [Header("Vida")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private BarraVida barraVida;
    public int Health { get; private set; }

    [Header("Daño Visual")]
    [SerializeField] private float primerDañoThreshold = 0.7f;
    [SerializeField] private float segundoDañoThreshold = 0.3f;

    [Header("Ataque")]
    [SerializeField] private int damage = 3;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float attackRange = 1.5f;

    [Header("Drops")]
    [SerializeField] private GameObject objetoMuerte;
    [SerializeField] private Transform spawnObjeto;

    // ESTADO
    private float lastAttackTime;
    private bool primerDañoActivado = false;
    private bool segundoDañoActivado = false;
    private bool estaMuerto = false;

    // cOMPONENETES
    private Animator animator;
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private AudioManager audioManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void Start()
    {
        Health = maxHealth;
        barraVida?.IniciarBarraVida(maxHealth);
        BuscarJugador();
    }

    private void Update()
    {
        if (estaMuerto) return;
        if (player == null) { BuscarJugador(); return; }

        MantenerEnSuelo();
        ProcesarIA();
    }

    private void ProcesarIA()
    {
        float distancia = Vector2.Distance(transform.position, player.position);

        if (distancia <= attackRange)
        {
            IntentarAtacar();
            animator.SetBool("Camina", false);
        }
        else
        {
            MoverHaciaJugador();
            animator.SetBool("Camina", true);
        }

        if (flipToFacePlayer)
            MirarJugador();
    }

    private void MoverHaciaJugador()
    {
        Vector2 destino = new Vector2(player.position.x, sueloNivel);
        Vector2 nuevaPos = Vector2.MoveTowards(transform.position, destino, speed * Time.deltaTime);
        transform.position = nuevaPos;
    }

    private void IntentarAtacar()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        animator.SetTrigger("Atacar");

        PlayerHealth hp = player.GetComponent<PlayerHealth>();
        if (hp != null)
            hp.TomarDano(damage);
    }

    private void BuscarJugador()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        if (obj != null) player = obj.transform;
    }

    private void MirarJugador()
    {
        float dir = player.position.x - transform.position.x;
        sprite.flipX = dir > 0;
    }

    private void MantenerEnSuelo()
    {
        if (Mathf.Abs(transform.position.y - sueloNivel) > 0.01f)
            transform.position = new Vector3(transform.position.x, sueloNivel, transform.position.z);
    }

   //CONBATE
    public void TomarDano(int daño)
    {
        if (estaMuerto) return;

        Health -= daño;
        barraVida?.CambiarVidaActual(Health);

        float pct = (float)Health / maxHealth;

        if (!primerDañoActivado && pct <= primerDañoThreshold)
        {
            animator.SetTrigger("PrimerDaño");
            primerDañoActivado = true;
        }
        else if (!segundoDañoActivado && pct <= segundoDañoThreshold)
        {
            animator.SetTrigger("SegundoDaño");
            segundoDañoActivado = true;
        }

        if (Health <= 0)
            Morir();
    }

    private void Morir()
    {
        if (estaMuerto) return;
        estaMuerto = true;

        animator.SetTrigger("Muere");

        if (audioManager != null)
            audioManager.PlaySFX(audioManager.muerteEnemigo);

        GetComponent<Collider2D>().enabled = false;

        Invoke(nameof(FinalizarMuerte), 1f);
    }

    private void FinalizarMuerte()
    {
        if (objetoMuerte != null)
            Instantiate(objetoMuerte, spawnObjeto.position, Quaternion.identity);

        Destroy(gameObject);
    }

    // Daño por trigger
    private void OnTriggerEnter2D(Collider2D col)
    {
        Ataque atk = col.GetComponent<Ataque>();
        if (atk != null)
            TomarDano(atk.Daño);
    }
}
