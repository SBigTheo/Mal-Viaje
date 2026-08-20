using UnityEngine;

public class Dickgeon : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadHorizontal = 2f;
    [SerializeField] private float amplitud = 1f;
    [SerializeField] private float frecuencia = 2f;
    [SerializeField] private float alturaCentro = 2f;

    [Header("Disparo")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private float distanciaVision = 10f;
    [SerializeField] private float umbralProductoPunto = 0.7f;

    private Transform jugador;
    private Vector2 direccionMovimiento;

    private float tiempoInicio;
    private bool disparoRealizado;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        tiempoInicio = Time.time;
    }

    public void Inicializar(Vector2 direccion)
    {
        direccionMovimiento = direccion;
    }

    private void Update()
    {
        Movimiento();
        VerificarDisparo();
    }

    private void Movimiento()
    {
        float tiempo = Time.time - tiempoInicio;

        float x = transform.position.x +
                  direccionMovimiento.x *
                  velocidadHorizontal *
                  Time.deltaTime;

        float y = alturaCentro +
                  Mathf.Sin(tiempo * frecuencia) *
                  amplitud;

        transform.position = new Vector2(x, y);

        if (Mathf.Abs(transform.position.x) > 13f)
        {
            Destroy(gameObject);
        }
    }

    private bool JugadorDetectado()
    {
        float distancia = Vector2.Distance(transform.position, jugador.position);

        if (distancia > distanciaVision)
            return false;

        Vector2 direccionJugador =
            ((Vector2)jugador.position - (Vector2)transform.position).normalized;

        float dot = Vector2.Dot(direccionMovimiento, direccionJugador);

        return dot >= umbralProductoPunto;
    }

    private void VerificarDisparo()
    {
        float alturaActual = transform.position.y;

        if (alturaActual >= alturaCentro + amplitud - 0.05f)
        {
            if (!disparoRealizado && JugadorDetectado())
            {
                disparoRealizado = true;
                Disparar();
            }
        }

        if (alturaActual < alturaCentro)
        {
            disparoRealizado = false;
        }
    }

    private void Disparar()
    {
        GameObject nuevoPooyectile = Instantiate(
            proyectilPrefab,
            firePoint.position,
            Quaternion.identity);

        Pooyectile pooyectile =
            nuevoPooyectile.GetComponent<Pooyectile>();

        Vector2 direccion =
            ((Vector2)jugador.position -
             (Vector2)firePoint.position).normalized;

        pooyectile.Inicializar(direccion);
    }
}