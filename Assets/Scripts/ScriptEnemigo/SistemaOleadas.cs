using UnityEngine;

public class SistemaOleadas : MonoBehaviour
{
    [System.Serializable]
    public class Secuaces
    {
        public GameObject prefabEnemigo;
        public int cantidad;
        public int contadorSpawn;
    }

    [System.Serializable]
    public class Oleada
    {
        private Lis<Secuaces> Secuaces;
        public int cantidadOleada;
        public int contadorSpawn;
        public float intervaloSpawn;
        public bool esOleadaJefe;
    }

    [Header("Configuración de Oleadas")]
    public List<Oleada> oleadas;
    public int oleadaActual;
    
    [Header("Configuración de Spawneo")]
    public List<Transform> puntosSpawneo;
    public float intervaloEntreOleadas = 3f;
    private bool isSpawning = false;
    
    [Header("Mini Jefe")]
    public GameObject prefabMiniJefe;
    public Transform puntoSpawneoJefe;
    
    private int enemigosVivos;
    private int enemigosTotales;
    private float temporizadorSpawneo;
    private bool estaSpawneando;

    private void Start() 
    {
        
    }

    void CalcularCantidadOleadas()
    {
        
    }

    IEnumerator IniciarSiguienteOleada()
    {
        
    }

    IEnumerator SpawnearEnemigos()
    {
        
    }

    public void EnemigoEliminado()
    {
        
    }

    void CompletarOleadaActual()
    {
        
    }

    void SpawnearMiniJefe()
    {
        
    }

}
