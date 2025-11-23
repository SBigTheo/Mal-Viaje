using UnityEngine;

public class ProyectilJefe : MonoBehaviour
{   
    public int dano = 4;
    public float velocidad = 10f;
    public Vecto2 direccion = Vector2.right;
    public float tiempoVida = 3f;

    void Start()
    {
        Destroy(gameObject, tiempoVida);
    }
    void Update()
    {
        transform.Translate(direccion *velocidad* tiempoVida.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealt>();

            if (playerHealth != null)
            {
                playerHealth.TomarDano(dano);
            }
            Destroy(gameObject);
        } else if( other.CompareTag(Ground) || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
