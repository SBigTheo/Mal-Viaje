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

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (barraDeVida != null)
        {
            barraDeVida.IniciarBarraDeVida(vida);
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
        if ((jugador.position.x > transform.position.x && !miradaDer) || 
            (jugador.position.x < transform.position.x && miradaDer))
        {
            miradaDer = !miradaDer;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }
    }
}