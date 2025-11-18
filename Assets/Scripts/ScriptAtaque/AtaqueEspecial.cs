using UnityEngine;

public class AtaqueEspecial : Ataque
{
    public float multiplicarDaño = 2f;
    public float radioDeExpansion = 2f;
    public float fuerzaEmpuje = 15f;

    private SistemaCombo sistemaCombo;    
    protected override void Start()
    {
        base.Start();
        sistemaCombo = GetComponent<SistemaCombo>();
        esAtaqueEspecial = true;
    }
    
    public override void EjecutarAtaque()
    {
        if(!PuedeAtacar()) 
        {
            Debug.Log("No se pudo ejecutar el ataque especial: condiciones no cumplidas");
            return;
        }

        Vector2 direccionAtaque = playerController.GetLastMovementDirection();
        Vector2 posicionAtaque = (Vector2)transform.position + direccionAtaque * rango;

        PlayAnimacionAtaque("AtaqueEspecial");
        DetectarEnemigo(posicionAtaque, rango * 0.4f);
        IniciarColdown();

        sistemaCombo.ConsumirCombo();
        Debug.Log("Ataque ejecutado");
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


    protected override bool PuedeAtacar()
    {
        bool basePuede = base.PuedeAtacar();
        bool comboDisponible = sistemaCombo != null && sistemaCombo.PuedeEjecutarAtaqueEspecial();
        
        Debug.Log($"PuedeAtacar especial - Base: {basePuede}, Combo: {comboDisponible}");
        
        return basePuede && comboDisponible;
    }
}