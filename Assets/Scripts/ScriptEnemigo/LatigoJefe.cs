using UnityEngine;

public class LatigoJefe : MonoBehaviour
{
    public int dano = 15;
    public float duracion = 0.4f;

    public Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = true;
    }

    public void Iniciar(Vector3 pos, Vector2 direccion, Transform jefe)
    {
        transform.position = pos;

        // Rotación correcta
        transform.rotation = direccion.x > 0 ? 
            Quaternion.identity : 
            Quaternion.Euler(0, 0, 180);
    }

    private void Start()
    {
        Destroy(gameObject, duracion);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth ph = other.GetComponent<PlayerHealth>();
        if (ph == null) return;

        ph.TomarDano(dano);

        // Evita daño múltiple
        col.enabled = false;

        Debug.Log($"Latigo daño al jugador: {dano}");
    }
}
