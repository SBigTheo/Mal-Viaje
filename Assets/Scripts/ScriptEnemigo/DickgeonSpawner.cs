using UnityEngine;

public class DickgeonSpawner : MonoBehaviour
{
    [SerializeField] private Dickgeon dickgeonPrefab;
    [SerializeField] private float tiempoEntreSpawn = 15f;
    [SerializeField] private float posicionX = 12f;
    [SerializeField] private float posicionY = 2f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnDickgeon), 0f, tiempoEntreSpawn);
    }

    private void SpawnDickgeon()
    {
        bool apareceDerecha = Random.value > 0.5f;
        Vector2 posicion = new Vector2(apareceDerecha ? posicionX : -posicionX, posicionY);
        
        Dickgeon enemigo = Instantiate(dickgeonPrefab, posicion, Quaternion.identity);
        enemigo.Inicializar(apareceDerecha ? Vector2.left : Vector2.right);
    }
}