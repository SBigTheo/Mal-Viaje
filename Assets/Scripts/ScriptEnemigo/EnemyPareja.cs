using UnityEngine;

public class EnemyPareja : MonoBehaviour
{
    [Header("COnfiguracion Inicial")]
    public float speed = 1.5f;
    public bool flipToFacePlayer = true;
    private bool esJefe = false;
    public int punto = 10;

    [Header("VIda")]
    public int maxHealth = 10;
    public int currentHealth;
    [SerializeField] private BarraVida barraVida;
    private bool primerDañoActivado = false;
    private bool segundoDañoActivado = false;


    [Header("Dropeo de Objeto")]
    [SerializeField] private GameObject objetoMuerte;
    [SerializeField] private Transform spawnObjeto;

    [Header("Animaciones de Daño")]
    [SerializeField] private float primerDañoThreshold = 0.7f; // 70% de vida
    [SerializeField] private float segundoDañoThreshold = 0.3f; // 30% de vida
    [SerializeField] private float muerteAnimationDelay = 1.0f;

    [Header("Ataque")]
    private int damage = 3;
    private float attackCooldown = 0.5f;
    private float attackRange = 1.5f;
    private float lasAttackTime = 0f;
    private bool canAtack = true;

    private Animator animator;
    public Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private SistemaOleadas sistemaOleadas;
    private bool seMueve = false;
    private float sueloNivel = -2.5f;

    public void ConfigurarSistemaOleadas(SistemaOleadas sistema)
    {
        sistemaOleadas = sistema;
    }

    private void Awake() {
        
    }

    void Start()
    {
        
    }

    private void FixedUpdate() {
        
    }

    void TryFindPlayer()
    {
        
    }

    void FacePlayer()
    {
        if (player == null) return;
        float dir = player.position.x - transform.position.x;

        if (sprite != null)
        {
            sprite.flipX = dir > 0;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (dir > 0 ? -1 : 1);
            transform.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AttackPlayer()
    {
        if (player == null || !canAtack) return;
        
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TomarDano(damage);
            animator.SetBool("Atacar", true);

            canAtack = false;
            lasAttackTime = Time.time;
        }
    }

    void OnDestroy()
    {
        player = null;
    }

    public void TomarDano()
    {
        
    }

    public void Soltarobjeto()
    {
        
    }

    private void AnimacionesDano()
    {
        
    }

    private void PlayAnimacionesDano()
    {
        
    }

    void Morir()
    {
        
    }

    private void Muerto()
    {
        
    }

    public float GetHealthPercentage()
    {
        
    }
}
