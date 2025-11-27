using UnityEngine;

public class Patada : Ataque
{
    [SerializeField] private float fuerzaRetroceso = 8f;

    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")
                        .GetComponent<AudioManager>();
    }

    public override void EjecutarAtaque()
    {
        if (!PuedeAtacar()) return;

        Vector2 direccion = playerController.GetLastMovementDirection();
        Vector2 posAtaque = (Vector2)transform.position + direccion * (Rango * 0.8f);

        PlayAnimacionAtaque("AtaquePatada");
        DetectarEnemigo(posAtaque, Rango * 0.4f);
        IniciarCooldown();
    }

    protected override void OnEnemyHit(GameObject enemigo)
    {
        audioManager.PlaySFX(audioManager.patada);

        if (enemigo.TryGetComponent(out Rigidbody2D rb))
        {
            Vector2 direccion = playerController.GetLastMovementDirection();
            rb.AddForce(direccion * fuerzaRetroceso, ForceMode2D.Impulse);
        }
    }
}
