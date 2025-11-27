using UnityEngine;

public class Golpe : Ataque
{
    [SerializeField] private float fuerzaEmpuje = 5f;

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
        Vector2 posAtaque = (Vector2)transform.position + direccion * Rango;

        PlayAnimacionAtaque("AtaqueGolpe");
        DetectarEnemigo(posAtaque, Rango * 0.3f);
        IniciarCooldown();
    }

    protected override void OnEnemyHit(GameObject enemigo)
    {
        audioManager.PlaySFX(audioManager.piña);

        if (enemigo.TryGetComponent(out Rigidbody2D rb))
        {
            Vector2 direccion = playerController.GetLastMovementDirection();
            rb.AddForce(direccion * fuerzaEmpuje, ForceMode2D.Impulse);
        }
    }
}
