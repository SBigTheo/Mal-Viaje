using UnityEngine;
using System.Collections;

public class AtaqueEspecial : Ataque
{
    [SerializeField] private float multiplicarDaño = 2f;
    [SerializeField] private float radioDeExpansion = 2f;
    [SerializeField] private float fuerzaEmpuje = 15f;
    [SerializeField] private float tiempoDeCarga = 0.9f;

    private SistemaCombo sistemaCombo;
    private AudioManager audioManager;

    protected override void Start()
    {
        base.Start();
        sistemaCombo = GetComponent<SistemaCombo>();
        audioManager = GameObject.FindGameObjectWithTag("Audio")
                       .GetComponent<AudioManager>();
    }

    public override void EjecutarAtaque()
    {
        if (!PuedeAtacar())
        {
            Debug.Log("Ataque especial no permitido");
            return;
        }

        audioManager.PlaySFX(audioManager.golpeEspecial);
        StartCoroutine(SecuenciaEspecial());
    }

    private IEnumerator SecuenciaEspecial()
    {
        IniciarAnimacionCarga();
        yield return new WaitForSeconds(tiempoDeCarga);
        EjecutarExplosion();
    }

    private void IniciarAnimacionCarga()
    {
        Animator anim = GetComponent<Animator>();
        if (anim != null)
            anim.SetTrigger("CargaAtaqueEspecial");
    }

    private void EjecutarExplosion()
    {
        Vector2 direccion = playerController.GetLastMovementDirection();
        Vector2 posAtaque = (Vector2)transform.position + direccion * Rango;

        PlayAnimacionAtaque("AtaqueEspecial");
        DetectarEnemigo(posAtaque, Rango * 0.4f);

        IniciarCooldown();
        sistemaCombo.ConsumirCombo();
    }

    protected override void OnEnemyHit(GameObject enemigo)
    {
        if (enemigo.TryGetComponent(out Rigidbody2D rb))
        {
            Vector2 direccion = playerController.GetLastMovementDirection();
            rb.AddForce(direccion * fuerzaEmpuje, ForceMode2D.Impulse);
        }
    }

    protected override bool PuedeAtacar()
    {
        return base.PuedeAtacar() &&
               sistemaCombo != null &&
               sistemaCombo.PuedeEjecutarAtaqueEspecial();
    }
}
