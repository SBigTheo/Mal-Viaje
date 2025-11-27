using UnityEngine;

public class LatigoJefe : MonoBehaviour
{
    private Vector2 direccion;
    private float velocidad = 8f;
    private float tiempoVida = 1.2f;
    private float tiempoInicio;

    public void Iniciar(Vector2 dir)
    {
        direccion = dir.normalized;
        tiempoInicio = Time.time;
    }

    private void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime);

        if (Time.time >= tiempoInicio + tiempoVida)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth ph = collision.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TomarDano(15);
            }

            Destroy(gameObject);
        }
    }
}