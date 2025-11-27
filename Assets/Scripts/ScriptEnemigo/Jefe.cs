using UnityEngine;

public class Jefe : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb2D;
    private Transform jugador;

    [Header("Vida")]
    [SerializeField] private float vidaMaxima = 100f;
    [SerializeField] private BarraVida barraVida;
    private float vidaActual;
    private bool estaMuerto = false;

    public float VidaActual => vidaActual; // Getter

    [Header("Ataque")]
    [SerializeField] private Transform controladorAtaque;
    [SerializeField] private float radioAtaque = 1.5f;
    [SerializeField] private int danoAtaque = 5;
    [SerializeField] private float cooldownAtaque = 2f;
    private float timerAtaque = 0f;


    [Header("Ataque Largo")]
    [SerializeField] private Transform puntoAtaqueLargo;
    [SerializeField] private GameObject prefabLatigo;

    [Header("Dropeo de Objeto")]
    [SerializeField] private GameObject objetoMuerte;
    [SerializeField] private Transform spawnObjeto;

    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento = 2f;
    [SerializeField] private float distanciaDeteccion = 10f;
    [SerializeField] private float distanciaAtaque = 3f;
    [SerializeField] private float distanciaParada = 2f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        vidaActual = vidaMaxima;

        GameObject jugadorObj = GameObject.FindGameObjectWithTag("Player");
        if (jugadorObj != null)
            jugador = jugadorObj.transform;

        barraVida?.IniciarBarraVida(vidaMaxima);
    }

    private void Update()
    {
        if (jugador == null || estaMuerto) return;

        timerAtaque += Time.deltaTime;

        float distancia = Vector2.Distance(transform.position, jugador.position);
        animator.SetFloat("DistanciaJugador", distancia);

        if (distancia <= distanciaDeteccion && distancia > distanciaAtaque)
        {
            PerseguirJugador();
        }
        else if (distancia <= distanciaAtaque && distancia > distanciaParada)
        {
            DetenerMovimiento();
            IntentarAtacar(distancia);
        }
        else if (distancia <= distanciaParada)
        {
            DetenerMovimiento();
            IntentarAtacar(distancia);
        }
        else
        {
            DetenerMovimiento();
        }

        MirarJugador();
    }

      // CONFIGURACION DE MOVIMIENTO
    private void PerseguirJugador()
    {
        animator.SetBool("Caminando", true);

        Vector2 direccion = (jugador.position - transform.position).normalized;

        rb2D.linearVelocity = new Vector2(direccion.x * velocidadMovimiento * Time.deltaTime * 60f,
                                            rb2D.linearVelocity.y);
    }

    private void DetenerMovimiento()
    {
        animator.SetBool("Caminando", false);
        rb2D.linearVelocity = new Vector2(0, rb2D.linearVelocity.y);
    }

    private void MirarJugador()
    {
        if (jugador == null) return;

        float direccionX = jugador.position.x - transform.position.x;

        if (direccionX > 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

      // CONFIGURACION DE ATAQUE
    public void IntentarAtacar(float distancia)
    {
        if (timerAtaque < cooldownAtaque) return;

        if (distancia <= distanciaParada)
        {
            animator.SetTrigger("AtacarCorto");
        }
        else if (distancia <= distanciaAtaque)
        {
            animator.SetTrigger("AtacarCorto");
        }
        else
        {
            animator.SetTrigger("AtacarLargo");
        }

        timerAtaque = 0f; // reiniciar cooldown
    }

    public void AtaqueCorto()
    {
        if (estaMuerto) return;

        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque);

        foreach (Collider2D colision in objetos)
        {
            if (colision.CompareTag("Player"))
            {
                PlayerHealth hp = colision.GetComponent<PlayerHealth>();
                hp?.TomarDano(danoAtaque);
                break;
            }
        }
    }

    public void AtaqueLargo()
    {
        if (estaMuerto) return;

        GameObject latigo = Instantiate(prefabLatigo, puntoAtaqueLargo.position, Quaternion.identity);

        Vector2 direccion = transform.localScale.x < 0 ? Vector2.right : Vector2.left;

        LatigoJefe lj = latigo.GetComponent<LatigoJefe>();
        lj.Iniciar(puntoAtaqueLargo.position, direccion, transform);
    }

    // CONFIGURACION DE LA VIDA
    public void TomarDano(float dano)
    {
        if (estaMuerto) return;

        vidaActual -= dano;
        vidaActual = Mathf.Clamp(vidaActual, 0, vidaMaxima);

        barraVida?.CambiarVidaActual(vidaActual);

        if (vidaActual <= 0)
            Morir();
        else
            animator.SetTrigger("Dano");
    }

    private void Morir()
    {
        estaMuerto = true;

        animator.SetTrigger("Muerte");
        rb2D.simulated = false;

        GameFlowManager.Instance?.RegisterEnemyKill();

        enabled = false;
    }

    public void SoltarObjeto()
    {
        if (objetoMuerte != null)
            Instantiate(objetoMuerte, spawnObjeto.position, Quaternion.identity);
    }

    public void Muerte()
    {
        Destroy(gameObject);
    }

    //dEBUG DE GIZMOS PARA VISUALIZAR
    private void OnDrawGizmos()
    {
        if (controladorAtaque != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(controladorAtaque.position, radioAtaque);
        }

        if (puntoAtaqueLargo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(puntoAtaqueLargo.position, radioAtaque);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccion);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distanciaAtaque);
    }
}
