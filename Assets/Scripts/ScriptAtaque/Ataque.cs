using System.Collections;
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

    protected bool onCooldown = false;
    protected float ultimoAtaque = 0f;
    protected PlayerController playerController;


    protected virtual void Start()
    { }

    public abstract void ejecutarAtaque()
    { }

    protected virtual void PuedeAtacar()
    { }
    protected virtual void IniciarColdown()
    { }
    protected virtual void PlayAnimacionAtaque(string trigerAnimacion)
    { }
    protected virtual void DetectarEnemigo(Vector2 posicionAtaque, float radio)
    { }
    protected virtual void OnEnemyHit(GameObject enemigo)
    { }
    private IEnumerator RutinaCooldown()
    { }
    public bool EstaEnCooldown()
    { }
}
