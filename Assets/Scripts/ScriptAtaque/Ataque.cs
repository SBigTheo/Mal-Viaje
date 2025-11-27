using UnityEngine;
using System.Collections.Generic;

public abstract class Ataque : MonoBehaviour
{
    [Header("Configuración Base")]
    [SerializeField] private string nombreAtaque;
    [SerializeField] private int daño = 1;
    [SerializeField] private float rango = 1.2f;
    [SerializeField] private float cooldown = 1.2f;
    [SerializeField] private KeyCode teclaAtaque;
    [SerializeField] private LayerMask capaEnemigo;

    [Header("Combos")]
    [SerializeField] private bool esAtaqueEspecial = false;
    [SerializeField] private List<SistemaCombo> combosQueActivanAtaque = new List<SistemaCombo>();

    // variables propias
    private bool enCooldown = false;
    private float contadorCooldown = 0f;
    protected PlayerController playerController;

    // Getters
    public string Nombre => nombreAtaque;
    public float Rango => rango;
    public int Daño => daño;
    public KeyCode TeclaAtaque => teclaAtaque;
    public bool EsAtaqueEspecial => esAtaqueEspecial;

    protected virtual void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (this is Golpe || this is Patada)
            esAtaqueEspecial = false;
    }

    protected virtual void Update()
    {
        ActualizarCooldown();
    }

    private void ActualizarCooldown()
    {
        if (!enCooldown)
            return;

        contadorCooldown -= Time.deltaTime;

        if (contadorCooldown <= 0f)
            enCooldown = false;
    }

    protected virtual bool PuedeAtacar()
    {
        return !enCooldown && Time.timeScale > 0f;
    }

    protected void IniciarCooldown()
    {
        enCooldown = true;
        contadorCooldown = cooldown;
    }

    public bool EstaEnCooldown() => enCooldown;

    //ATAQUE
    public abstract void EjecutarAtaque();

    protected void PlayAnimacionAtaque(string trigger)
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger(trigger);
    }

    protected void DetectarEnemigo(Vector2 posicion, float radio)
    {
        Collider2D[] enemigos = Physics2D.OverlapCircleAll(posicion, radio, capaEnemigo);

        foreach (Collider2D enemigo in enemigos)
        {
            if (enemigo.TryGetComponent(out Jefe jefe))
            {
                jefe.TomarDano(daño);
                OnEnemyHit(enemigo.gameObject);
                continue;
            }

            if (enemigo.TryGetComponent(out EnemyPareja pareja))
            {
                pareja.TomarDano(daño);
                OnEnemyHit(enemigo.gameObject);
                continue;
            }

            if (enemigo.TryGetComponent(out BusEnemyHealth bus))
            {
                bus.TakeDamage(daño);
                OnEnemyHit(enemigo.gameObject);
                continue;
            }

            if (enemigo.CompareTag("Enemy") &&
                enemigo.TryGetComponent(out EnemyHealth health))
            {
                health.TakeDamage(daño);
                OnEnemyHit(enemigo.gameObject);
            }
        }
    }

    protected virtual void OnEnemyHit(GameObject enemigo)
    {
        Debug.Log($"Enemigo afectado: {enemigo.name}");
    }
}
