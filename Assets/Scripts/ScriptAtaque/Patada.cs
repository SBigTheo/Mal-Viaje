using UnityEngine;

public class Patada : Ataque
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private float fuerzaRetroceso = 8f;
    public override void EjecutarAtaque()
    {
        if (!PuedeAtacar()) return;

        Vector2 direccionAtaque = playerController.GetLastMovementDirection();
        Vector2 posicionAtaque = (Vector2)transform.position + direccionAtaque * (rango * 0.8f);

        PlayAnimacionAtaque("AtaquePatada");
        DetectarEnemigo(posicionAtaque, rango * 0.4f);
        IniciarColdown();
    }
    protected override void OnEnemyHit(GameObject enemigo)
    {
        audioManager.PlaySFX(audioManager.patada);
        // Empujar al enemigo
        Vector2 direccionEmpuje = playerController.GetLastMovementDirection();
        Rigidbody2D rbEnemigo = enemigo.GetComponent<Rigidbody2D>();
        if (rbEnemigo != null)
        {
            rbEnemigo.AddForce(direccionEmpuje * fuerzaRetroceso, ForceMode2D.Impulse);
        }
    }
}
