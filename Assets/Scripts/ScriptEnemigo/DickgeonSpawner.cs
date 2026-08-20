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

        Vector2 posicion;

        if (apareceDerecha)
        {
            posicion = new Vector2(posicionX, posicionY);
        }
        else
        {
            posicion = new Vector2(-posicionX, posicionY);
        }

        Dickgeon enemigo =
            Instantiate(dickgeonPrefab,
                        posicion,
                        Quaternion.identity);

        if (apareceDerecha)
        {
            enemigo.Inicializar(Vector2.left);
        }
        else
        {
            enemigo.Inicializar(Vector2.right);
        }
    }
}