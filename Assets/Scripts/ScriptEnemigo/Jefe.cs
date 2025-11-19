using UnityEngine;

public class Jefe : MonoBehaviour
{
    private Animator animator;
    public Rigidbody2D rb2D;
    private Transform jugador;
    private bool miradaDer = true;

    [Header("Vida")]
    [SerializeField] private float vida;
    [SerializeField] private BarraVida barraDeVida;

    [Header("Ataque")]
    [SerializeField] private Transform ControladorAtaque;
    [SerializeField] private float radioAtaque;
    [SerializeField]private int danoAtaque;
    [SerializeField] private float cooldownAtaque = 2f;
    private float tiempoUltimoAtaque;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (jugador == null)
    {
        Debug.LogError("No se encontró el jugador con tag 'Player'");
    }
    
    if (barraDeVida != null)
    {
        barraDeVida.IniciarBarraDeVida(vida);
    }
    else
    {
        Debug.LogWarning("BarraDeVida no asignada en el inspector");
    }
    }

    public void TomarDano(float dano)
    {
        vida -= dano;

        if (barraDeVida != null)
        {
            barraDeVida.CambiarVidaActual(vida);
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
        if (jugador == null);

        if ((jugador.position.x > transform.position.x && !miradaDer) || 
            (jugador.position.x < transform.position.x && miradaDer))
        {
            miradaDer = !miradaDer;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }
    }

    public void Ataque()
    {
        if (ControladorAtaque == null)
        {
            Debug.Log("NOse se asino el puto controlador");
        }
        Collider2D[] objetos = Physics2D.OverlapCircleAll(ControladorAtaque.position, radioAtaque);
        foreach (Collider2D colision in objetos)
        {
            if (colision.CompareTag("Player"))
            {
                colision.GetComponent<PlayerHealth>().TomarDano(danoAtaque);
            }
        }
    }
    
    public void IntentarAtacar()    
    {
        if(Time.time >= tiempoUltimoAtaque + cooldownAtaque)
        {
            Ataque();
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
        if (ControladorAtaque == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ControladorAtaque.position, radioAtaque);
    }
}