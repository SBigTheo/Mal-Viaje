using UnityEngine;

public class Dickgeon : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadHorizontal = 2f;
    [SerializeField] private float amplitud = 1f;
    [SerializeField] private float frecuencia = 2f;
    [SerializeField] private float alturaCentro = 2f;

    [Header("Disparo")]
    [SerializeField] private GameObject pooyectilePrefab;
    [SerializeField] private float distanciaVision = 10f;
    [SerializeField] private float umbralProductoPunto = -0.3f;

    private Transform jugador;
    private Vector2 direccionMovimiento;
    private float tiempoInicio;
    private bool disparoRealizado;
    private Transform firePoint;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
        tiempoInicio = Time.time;
        spriteRenderer = GetComponent<SpriteRenderer>();
        CrearFirePoint();
        ActualizarOrientacion();
    }

    private void CrearFirePoint()
    {
        GameObject firePointObj = new GameObject("FirePoint");
        firePointObj.transform.SetParent(transform);
        
        float offsetX = direccionMovimiento.x * 0.5f;
        firePointObj.transform.localPosition = new Vector3(offsetX, 0.2f, 0);
        
        firePoint = firePointObj.transform;
    }

    public void Inicializar(Vector2 direccion)
    {
        direccionMovimiento = direccion;
        
        if (firePoint != null)
        {
            float offsetX = direccion.x * 0.5f;
            firePoint.localPosition = new Vector3(offsetX, 0.2f, 0);
        }
        
        ActualizarOrientacion();
    }

    private void ActualizarOrientacion()
    {
        if (spriteRenderer == null)
            return;

        if (direccionMovimiento.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direccionMovimiento.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void Update()
    {
        MovimientoSinusoidal();
        VerificarDisparo();
    }

    private void MovimientoSinusoidal()
    {
        float tiempo = Time.time - tiempoInicio;
        
        float x = transform.position.x + direccionMovimiento.x * velocidadHorizontal * Time.deltaTime;
        float y = alturaCentro + Mathf.Sin(tiempo * frecuencia) * amplitud;

        transform.position = new Vector2(x, y);

        if (Mathf.Abs(transform.position.x) > 13f)
        {
            Destroy(gameObject);
        }
    }

    private bool JugadorDetectado()
    {
        if (jugador == null) return false;
        
        float dx = jugador.position.x - transform.position.x;
        float dy = jugador.position.y - transform.position.y;
        float distancia = Mathf.Sqrt(dx * dx + dy * dy);

        if (distancia > distanciaVision)
            return false;

        Vector2 direccionJugador = new Vector2(dx / distancia, dy / distancia);
        float productoPunto = direccionMovimiento.x * direccionJugador.x + direccionMovimiento.y * direccionJugador.y;

        return productoPunto <= umbralProductoPunto;
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
        if (pooyectilePrefab == null)
            return;

        Vector2 puntoDisparo = firePoint != null ? (Vector2)firePoint.position : (Vector2)transform.position;
        
        GameObject nuevoPooyectile = Instantiate(pooyectilePrefab, puntoDisparo, Quaternion.identity);
        Pooyectile pooyectile = nuevoPooyectile.GetComponent<Pooyectile>();
        
        if (pooyectile == null)
        {
            Destroy(nuevoPooyectile);
            return;
        }
        
        float dx = jugador.position.x - puntoDisparo.x;
        float dy = jugador.position.y - puntoDisparo.y;
        float distancia = Mathf.Sqrt(dx * dx + dy * dy);
        
        Vector2 direccion = distancia < 0.01f ? Vector2.up : new Vector2(dx / distancia, dy / distancia);
        pooyectile.Inicializar(direccion);
    }

    private void OnDrawGizmos()
    {
        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(firePoint.position, 0.2f);
        }
    }
}