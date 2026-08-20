using UnityEngine;

public class Pooyectile : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadHorizontal = 8f;
    [SerializeField] private float velocidadCaida = 2f;

    [Header("Daño")]
    [SerializeField] private int dano = 1;

    private Vector2 velocidad;
    private float tiempoVida = 0f;
    private const float TIEMPO_MAXIMO = 5f;

    public void Inicializar(Vector2 direccionDisparo)
    {
        velocidad = direccionDisparo * velocidadHorizontal;
    }

    private void Update()
    {
        tiempoVida += Time.deltaTime;
        
        velocidad.y -= velocidadCaida * Time.deltaTime;
        transform.position += (Vector3)(velocidad * Time.deltaTime);

        if (transform.position.y <= -4f || tiempoVida >= TIEMPO_MAXIMO)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TomarDano(dano);
        }

        Destroy(gameObject);
    }
}