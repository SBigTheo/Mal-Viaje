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
    [Header("Configuracion Del Combo")]
    private List<Combo> combos = new List<Combo>();

    [Header("Estado De los Combos")]

    public int indiceCombo = 0;
    public Combo comboActivo = null;
    public float tiempoEntreAtaque = 0f;
    public bool comboDisponible = false;

    private PlayerController playerController;
    private List<Ataque> ataquesBasicos = new List<Ataque>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void ProcesarAtaques()
    {
        
    }

    void CompletarCombo()
    {
        
    }
}
