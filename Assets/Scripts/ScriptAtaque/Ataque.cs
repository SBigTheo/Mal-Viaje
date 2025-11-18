using System.Collections;
using System.Collections.Generic;
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

    [Header("Combos")]
    public bool esAtaqueEspecial = false;
    public List<SistemaCombo> combosQueActivanAtaque = new List<SistemaCombo>();

    protected bool enCooldown = false;
    protected float ultimoAtaque = 0f;
    protected PlayerController playerController;

    protected virtual void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (this is Golpe || this is Patada)
        {
            esAtaqueEspecial = false;
        }
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

        Debug.Log($"Ataque '{nombreAtaque}' detectó {enemigosGolpeados.Length} objetivos");

        foreach (Collider2D enemigo in enemigosGolpeados)
        {
            Debug.Log($"Golpeando: {enemigo.gameObject.name}");

            BusEnemyHealth busEnemyHealth = enemigo.GetComponent<BusEnemyHealth>();
            if (busEnemyHealth != null)
            {
                Debug.Log($"BusEnemy golpeado con {daño} de daño!");
                busEnemyHealth.TakeDamage(daño);
                OnEnemyHit(enemigo.gameObject);
                continue;
            }

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
        Debug.Log($"Enemigo afectado: {enemigo.name}");
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