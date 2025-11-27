using System.Collections.Generic;
using UnityEngine;

public class SistemaCombo : MonoBehaviour
{
    [System.Serializable]
    public class Combo
    {
        public string nombreCombo;
        public List<KeyCode> secuencia;
        public Ataque ataqueEspecial;
        public float tiempoEntreAtaques;
    }

    [Header("Configuración de Combos")]
    [SerializeField] private List<Combo> combos = new List<Combo>();

    private int indiceCombo = 0;
    private Combo comboActivo = null;
    private float tiempoUltimoInput = 0f;
    private bool comboDisponible = false;

    private List<Ataque> ataquesBasicos = new List<Ataque>();

    private void Start()
    {
        var todosLosAtaques = GetComponents<Ataque>();

        foreach (var ataque in todosLosAtaques)
            if (!ataque.EsAtaqueEspecial)
                ataquesBasicos.Add(ataque);
    }

    private void Update()
    {
        // si se acabó el tiempo → reset
        if (comboActivo != null &&
            Time.time - tiempoUltimoInput > comboActivo.tiempoEntreAtaques)
        {
            ResetearCombo();
        }

        foreach (Ataque ataque in ataquesBasicos)
        {
            if (Input.GetKeyDown(ataque.TeclaAtaque))
            {
                ProcesarAtaques(ataque.TeclaAtaque);
                break;
            }
        }
    }

    private void ProcesarAtaques(KeyCode tecla)
    {
        if (comboActivo == null)
        {
            foreach (Combo combo in combos)
            {
                if (combo.secuencia.Count > 0 && combo.secuencia[0] == tecla)
                {
                    comboActivo = combo;
                    indiceCombo = 1;
                    tiempoUltimoInput = Time.time;
                    return;
                }
            }
        }
        else
        {
            if (indiceCombo < comboActivo.secuencia.Count &&
                comboActivo.secuencia[indiceCombo] == tecla)
            {
                indiceCombo++;
                tiempoUltimoInput = Time.time;

                if (indiceCombo >= comboActivo.secuencia.Count)
                {
                    CompletarCombo();
                }
            }
            else
            {
                ResetearCombo();
            }
        }
    }

    private void CompletarCombo()
    {
        comboDisponible = true;

        if (comboActivo.ataqueEspecial != null &&
            !comboActivo.ataqueEspecial.EstaEnCooldown())
        {
            comboActivo.ataqueEspecial.EjecutarAtaque();
        }

        ResetearCombo();
    }

    private void ResetearCombo()
    {
        comboActivo = null;
        indiceCombo = 0;
        comboDisponible = false;
    }

    public bool PuedeEjecutarAtaqueEspecial() => comboDisponible;

    public void ConsumirCombo() => comboDisponible = false;
}
