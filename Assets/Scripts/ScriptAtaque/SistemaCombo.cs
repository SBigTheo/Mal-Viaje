using System.Collections.Generic;
using System.Data.Common;
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
    [Header("Configuracion Del Combo")]
    private List<Combo> combos = new List<Combo>();

    [Header("Estado De los Combos")]

    public int indiceCombo = 0;
    public Combo comboActivo = null;
    public float tiempoEntreAtaque = 0f;
    public bool comboDisponible = false;

    private PlayerController playerController;
    private List<Ataque> ataquesBasicos = new List<Ataque>();

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        Ataque[] todoAtaque = GetComponents<Ataque>();

        foreach(Ataque ataque in todoAtaque)
        {
            if(ataque is Golpe || ataque is Patada)
            {
                ataquesBasicos.Add(ataque);
            }
        }
    }

    private void Update()
    {
        if(comboActivo != null && Time.time - tiempoEntreAtaque > comboActivo.tiempoEntreAtaques)
        {
            ResetearCombo();
        }

        foreach(Ataque ataque in ataquesBasicos)
        {
            if (Input.GetKeyDown(ataque.teclaAtaque))
            {
                ProcesarAtaques(ataque.teclaAtaque);
                break;
            }
        }
    }

    void ProcesarAtaques(KeyCode teclaPresionada)
    {
        if(comboActivo == null)
        {
            foreach(Combo combo in combos)
            {
                if(combo.secuencia.Count > 0 && combo.secuencia[0] == teclaPresionada)
                {
                    comboActivo = combo;
                    indiceCombo = 1;
                    tiempoEntreAtaque = Time.time;
                    return;
                }
            }
        }
    }

    void CompletarCombo()
    {
        
    }

    private void ResetearCombo()
    {
        comboActivo = null;
        indiceCombo = 0;
        comboDisponible = false;
    }
}
