using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Ataque : MonoBehaviour
{
    [Header("Configuracion Base")]
    public string nombreAtaque;
    public int daño = 1;
    public float rango = 1.2f;
    public float cooldown = 1.2f;
    public KeyCode teclaAtaque;
    public LayerMask capaEnemigo;

    protected bool enCooldown = false;
    protected float ultimoAtaque = 0f;
    protected PlayerController playerController;


    protected virtual void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    public abstract void EjecutarAtaque();
    protected virtual bool PuedeAtacar()
    {
        return !enCooldown && Time.timeScale > 0f;
    }
    protected virtual void IniciarColdown()
    {
        enCooldown = true;
        ultimoAtaque = Time.time;
        StartCoroutine(RutinaCooldown());
    }
    protected virtual void PlayAnimacionAtaque(string trigerAnimacion)
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger(trigerAnimacion);
        }
    }
    protected virtual void DetectarEnemigo(Vector2 posicionAtaque, float radio)
    {
        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(posicionAtaque, radio, capaEnemigo);
        foreach (Collider2D enemigo in enemigosGolpeados)
        {
            if (enemigo.CompareTag("Enemy"))
            {
                EnemyHealth saludEnemigo = enemigo.GetComponent<EnemyHealth>();
                if (saludEnemigo != null)
                {
                    saludEnemigo.TakeDamage(daño);
                    OnEnemyHit(enemigo.gameObject);
                }
            }
        }
    }
    protected virtual void OnEnemyHit(GameObject enemigo)
    {
        //Para que la sobreescriban
    }
    private IEnumerator RutinaCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        enCooldown = false;
    }
    public bool EstaEnCooldown()
    {
        return enCooldown;
    }
}
