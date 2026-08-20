using UnityEngine;

public class Pooyectile : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 8f;
    [SerializeField] private float velocidadCaida = 2f;

    [Header("Daño")]
    [SerializeField] private int dano = 1;

    private Vector2 velocidadMovimiento;

    public void Inicializar(Vector2 direccionDisparo)
    {
        direccionDisparo.Normalize();

        velocidadMovimiento = direccionDisparo * velocidad;
    }

    private void Update()
    {
        velocidadMovimiento.y -= velocidadCaida * Time.deltaTime;

        transform.position += (Vector3)(velocidadMovimiento * Time.deltaTime);

        if (transform.position.y <= -4f)
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