using UnityEngine;

public class AtaqueEspecial : Ataque
{
    public float multiplicarDaño = 2f;
    public float radioDeExpnsion= 2f;
    public float fuerzaEmpuje = 15f;

    private SistemaCombo sistemaCombo;
    protected override void Start()
    {
        base.Start();
        sistemaCombo = GetComponent<SistemaCombo>();
    }
    public override void EjecutarAtaque()
    {
        if(!PuedeAtacar()) return;

        if(sistemaCombo == null || !sistemaCombo.PuedeEjecutarAtaqueEspecial()) return;

        Vector2 direccionAtaque = playerController.GetLastMovementDirection();
        Vector2 posicionAtaque = (Vector2)transform.position + direccionAtaque * rango;

        PlayAnimacionAtaque("AtaqueEspecial");
        DetectarEnemigo(posicionAtaque, rango * 0.4f);
        IniciarColdown();

        sistemaCombo.ConsumirCombo();
    }

    
}
