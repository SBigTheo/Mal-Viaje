using UnityEngine;

public class Golpe : Ataque
{
    public float fuerzaEmpuje = 5f;
    public override void EjecutarAtaque()
    {
        if (!PuedeAtacar()) return;

        Vector2 direccionAtaque = playerController.GetLastMovementDirection();
        Vector2 posicionAtaque = (Vector2)transform.position + direccionAtaque * rango;

        PlayAnimacionAtaque("AtaqueGolpe");
        DetectarEnemigo(posicionAtaque, rango * 0.3f);
        IniciarColdown();
    }
    protected override void OnEnemyHit(GameObject enemigo)
    {
        // Empujar al enemigo
        Vector2 direccionEmpuje = playerController.GetLastMovementDirection();
        Rigidbody2D rbEnemigo = enemigo.GetComponent<Rigidbody2D>();
        if (rbEnemigo != null)
        {
            rbEnemigo.AddForce(direccionEmpuje * fuerzaEmpuje, ForceMode2D.Impulse);
        }
    }
}
