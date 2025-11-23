using UnityEngine;

public class Jefe : MonoBehaviour
{
    private Animator animator;
    public Rigidbody2D rb2D;
    private Transform jugador;
    // private bool miradaDer = true;

    [Header("Vida")]
    [SerializeField] public float vida;
    [SerializeField] private BarraVida barraVida;

    [Header("Ataque")]
    [SerializeField] private Transform ControladorAtaque;
    [SerializeField] private float radioAtaque;
    [SerializeField] private int danoAtaque;
    [SerializeField] private float cooldownAtaque = 2f;
    private float tiempoUltimoAtaque;

    [Header("Movimiento")]
    // [SerializeField] private float velocidadMovimiento = 3f;
    [SerializeField] private float distanciaDeteccion = 10f;
    [SerializeField] private float distanciaAtaque = 3f;
    [SerializeField] private float distanciaParada = 2f;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        
        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
        {
            jugador = jugadorObj.transform;
        }
        else
        {
            Debug.LogError("No se encontró el jugador con el tag 'Player'");
        }
        
        if (barraVida != null)
        {
            barraVida.IniciarBarraVida(vida);
        }
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector2.Distance(transform.position, jugador.position);
        animator.SetFloat("DistanciaJugador", distancia);

        if (distancia <= distanciaDeteccion && distancia > distanciaAtaque)
        {
            // PerseguirJugador();
            animator.SetBool("Caminando", true);
        }
        else if (distancia <= distanciaAtaque && distancia > distanciaParada)
        {
            animator.SetBool("Caminando", false);
            IntentarAtacar();
        }
        else if (distancia <= distanciaParada)
        {
            animator.SetBool("Caminando", false);
            // Retroceder();
            IntentarAtacar();
        }
        else
        {
            animator.SetBool("Caminando", false);
            rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
        }
        MirarJugador();
    }

    public void TomarDano(float dano)
    {
        vida -= dano;

        if (barraVida != null)
        {
            barraVida.CambiarVidaActual(vida);
        }

        if (vida <= 0)
        {
            animator.SetTrigger("Muerte");
        }
    }

    public void Muerte()
    {
        Destroy(gameObject);
    }

    public void MirarJugador()
{
    if (jugador == null) return;

    float direccionX = jugador.position.x - transform.position.x;
    
    if (direccionX > 0)
    {
        transform.localScale = new Vector3(-1, 1, 1);
    }
    else if (direccionX < 0)
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
}

    public void Ataque()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(ControladorAtaque.position, radioAtaque);
        foreach (Collider2D colision in objetos)
        {
            if (colision.CompareTag("Player"))
            {
                PlayerHealth playerHealth = colision.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TomarDano(danoAtaque);
                    Debug.Log("Jefe atacó al jugador!");
                }
                break;
            }
        }
    }
    
    public void IntentarAtacar()    
    {
        if(Time.time >= tiempoUltimoAtaque + cooldownAtaque)
        {
            animator.SetTrigger("AtacarCorto");
            tiempoUltimoAtaque = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ataque"))
        {
            Ataque ataque = other.GetComponent<Ataque>();
            if (ataque != null)
            {
                TomarDano(ataque.daño);
            }
        }
    }

    private void OnDrawGizmos() 
    {
        if (ControladorAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(ControladorAtaque.position, radioAtaque);
        }
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccion);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
    }
}