using System.Collections;
using UnityEngine;

public class AtaqueEspecial : Ataque
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public float multiplicarDaño = 2f;
    public float radioDeExpansion = 2f;
    public float fuerzaEmpuje = 15f;
    private float tiempoDeCarga = 0.9f;

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

        audioManager.PlaySFX(audioManager.golpeEspecial);

        StartCoroutine(SecuenciaDeAtaqueEspecial());
    }

    private IEnumerator SecuenciaDeAtaqueEspecial()
    {

        Debug.Log("Cargando el ataque");
        IniciarAniamcionCarga();

        yield return new WaitForSeconds(tiempoDeCarga);
        EjecutarTortaso();
    }

    private void IniciarAniamcionCarga()
    {
        Animator animacion = GetComponent<Animator>();
        if(animacion != null)
        {
            animacion.SetTrigger("CargaAtaqueEspecial");
        }

        Debug.Log("Cargando torta");
    }

    private void EjecutarTortaso()
    {
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