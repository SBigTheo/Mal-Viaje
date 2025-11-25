using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SistemaOleadas : MonoBehaviour
{
    [System.Serializable]
    public class GrupoEnemigos
    {
        public GameObject prefabEnemigo;
        public int cantidad;
        public int contadorSpawneo;
    }

    [System.Serializable]
    public class Oleada
    {
        public string nombreOleada;
        public List<GrupoEnemigos> gruposEnemigos;
        public int cuotaOleada; // Total de enemigos en esta oleada
        public float intervaloSpawneo;
        public int contadorSpawneo; // Cuántos enemigos han aparecido
        public bool esOleadaJefe;
    }

    [Header("Configuración de Oleadas")]
    public List<Oleada> oleadas;
    public int oleadaActual;
    
    [Header("Configuración de Spawneo")]
    public List<Transform> puntosSpawneo;
    public float intervaloEntreOleadas = 3f;
    
    [Header("Configuración Avanzada de Spawneo")]
    public float minDistanciaEntreEnemigos = 3f;
    public float distanciaMinimaDelJugador = 4f;
    public int maxIntentosSpawneo = 10;
    
    [Header("Mini Jefe")]
    public GameObject prefabMiniJefe;
    public Transform puntoSpawneoJefe;
    
    // Lista para controlar enemigos activos (del EnemySpawner)
    private List<GameObject> enemigosActivos = new List<GameObject>();
    
    private int enemigosVivos;
    private int enemigosTotales;
    private float temporizadorSpawneo;
    private bool estaSpawneando;
    
    // Eventos para UI y otros sistemas
    public System.Action<int> AlIniciarOleada;
    public System.Action<int> AlCompletarOleada;
    public System.Action AlAparecerJefe;
    public System.Action AlCompletarTodasOleadas;

    void Start()
    {
        CalcularCuotaOleadas();
        StartCoroutine(IniciarSiguienteOleada());
    }

    void Update()
    {
        if (estaSpawneando && oleadaActual < oleadas.Count)
        {
            temporizadorSpawneo -= Time.deltaTime;
            if (temporizadorSpawneo <= 0)
            {
                temporizadorSpawneo = oleadas[oleadaActual].intervaloSpawneo;
                StartCoroutine(SpawnearEnemigos());
            }
        }
    }

    void CalcularCuotaOleadas()
    {
        foreach (Oleada oleada in oleadas)
        {
            oleada.cuotaOleada = 0;
            foreach (GrupoEnemigos grupo in oleada.gruposEnemigos)
            {
                oleada.cuotaOleada += grupo.cantidad;
            }
        }
    }

    IEnumerator IniciarSiguienteOleada()
    {
        if (oleadaActual >= oleadas.Count)
        {
            // Todas las oleadas completadas
            AlCompletarTodasOleadas?.Invoke();
            yield break;
        }

        yield return new WaitForSeconds(intervaloEntreOleadas);
        
        Oleada oleadaActualObj = oleadas[oleadaActual];
        estaSpawneando = true;
        temporizadorSpawneo = 0f;
        enemigosVivos = oleadaActualObj.cuotaOleada;
        enemigosTotales = oleadaActualObj.cuotaOleada;
        
        // Limpiar lista de enemigos activos al iniciar nueva oleada
        enemigosActivos.Clear();
        
        AlIniciarOleada?.Invoke(oleadaActual + 1);
        
        Debug.Log($"¡Oleada {oleadaActual + 1} iniciada! Enemigos: {enemigosTotales}");
    }

    IEnumerator SpawnearEnemigos()
    {
        Oleada oleadaActualObj = oleadas[oleadaActual];
        
        foreach (GrupoEnemigos grupo in oleadaActualObj.gruposEnemigos)
        {
            if (grupo.contadorSpawneo < grupo.cantidad)
            {
                // Intentar encontrar una posición de spawneo válida
                Vector2 posicionSpawneo = ObtenerPosicionSpawneoValida();
                
                if (posicionSpawneo != Vector2.zero) // Vector2.zero indica que no se encontró posición válida
                {
                    // Instanciar enemigo
                    GameObject enemigo = Instantiate(grupo.prefabEnemigo, posicionSpawneo, Quaternion.identity);
                    
                    // Agregar a la lista de enemigos activos
                    enemigosActivos.Add(enemigo);
                    
                    // Configurar referencia al SistemaOleadas para EnemyFollow
                    EnemyFollow scriptEnemy = enemigo.GetComponent<EnemyFollow>();
                    if (scriptEnemy != null)
                    {
                        // Si tu EnemyFollow no tiene ConfigurarSistemaOleadas, podemos agregar una referencia directa
                        // o simplemente no hacer nada si no es necesario
                        ConfigurarEnemigo(scriptEnemy, enemigo);
                    }
                    
                    grupo.contadorSpawneo++;
                    oleadaActualObj.contadorSpawneo++;
                    
                    yield return new WaitForSeconds(0.5f); // Pequeño delay entre spawneos
                }
                else
                {
                    Debug.LogWarning("No se pudo encontrar posición de spawneo válida");
                }
            }
        }

        // Verificar si hemos terminado de spawnear esta oleada
        if (oleadaActualObj.contadorSpawneo >= oleadaActualObj.cuotaOleada)
        {
            estaSpawneando = false;
        }
    }

    // Método para configurar el enemigo con referencia al sistema de oleadas
    void ConfigurarEnemigo(EnemyFollow enemyScript, GameObject enemigoObj)
    {
        // Opción 1: Si tu EnemyFollow tiene un método para setear el sistema de oleadas
        // enemyScript.SetSistemaOleadas(this);
        
        // Opción 2: Agregar componente personalizado para manejar la muerte
        EnemyOleadaManager oleadaManager = enemigoObj.GetComponent<EnemyOleadaManager>();
        if (oleadaManager == null)
        {
            oleadaManager = enemigoObj.AddComponent<EnemyOleadaManager>();
        }
        oleadaManager.Configurar(this, enemigoObj);
    }

    Vector2 ObtenerPosicionSpawneoValida()
    {
        // Limpiar lista de enemigos nulos
        LimpiarListaEnemigos();
        
        for (int i = 0; i < maxIntentosSpawneo; i++)
        {
            Vector2 posicionSpawneo = ObtenerPosicionSpawneoAleatoria();
            if (EsPosicionSpawneoValida(posicionSpawneo))
            {
                return posicionSpawneo;
            }
        }
        
        return Vector2.zero; // No se encontró posición válida
    }

    Vector2 ObtenerPosicionSpawneoAleatoria()
    {
        if (puntosSpawneo != null && puntosSpawneo.Count > 0)
        {
            // Usar puntos de spawneo definidos
            Transform puntoSpawneo = puntosSpawneo[Random.Range(0, puntosSpawneo.Count)];
            return puntoSpawneo.position;
        }
        else
        {
            // Usar sistema de áreas de spawneo del EnemySpawner original
            bool spawnIzquierda = Random.value < 0.5f;
            
            if (spawnIzquierda)
            {
                float x = Random.Range(-15f, -11f);
                return new Vector2(x, -2.5f);
            }
            else
            {
                float x = Random.Range(11f, 14.4f);
                return new Vector2(x, -2.5f);
            }
        }
    }

    bool EsPosicionSpawneoValida(Vector2 posicionSpawneo)
    {
        // Verificar distancia con otros enemigos
        foreach (GameObject enemigo in enemigosActivos)
        {
            if (enemigo == null) continue;
            if (Vector2.Distance(posicionSpawneo, enemigo.transform.position) < minDistanciaEntreEnemigos)
                return false;
        }

        // Verificar distancia con el jugador
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");
        if (jugador != null && Vector2.Distance(posicionSpawneo, jugador.transform.position) < distanciaMinimaDelJugador)
            return false;

        return true;
    }

    void LimpiarListaEnemigos()
    {
        for (int i = enemigosActivos.Count - 1; i >= 0; i--)
        {
            if (enemigosActivos[i] == null)
            {
                enemigosActivos.RemoveAt(i);
            }
        }
    }

    // Llamado cuando un enemigo es eliminado
    public void EnemigoEliminado(GameObject enemigo = null)
    {
        // Remover de la lista de enemigos activos si se proporciona
        if (enemigo != null && enemigosActivos.Contains(enemigo))
        {
            enemigosActivos.Remove(enemigo);
        }
        
        enemigosVivos--;
        
        // Verificar si la oleada actual está completa
        if (enemigosVivos <= 0 && oleadaActual < oleadas.Count)
        {
            CompletarOleadaActual();
        }
    }

    void CompletarOleadaActual()
    {
        Oleada oleadaActualObj = oleadas[oleadaActual];
        
        Debug.Log($"¡Oleada {oleadaActual + 1} completada!");
        AlCompletarOleada?.Invoke(oleadaActual + 1);
        
        // Limpiar lista de enemigos activos
        enemigosActivos.Clear();
        
        // Verificar si es oleada de jefe
        if (oleadaActualObj.esOleadaJefe)
        {
            SpawnearMiniJefe();
        }
        else
        {
            // Pasar a la siguiente oleada
            oleadaActual++;
            if (oleadaActual < oleadas.Count)
            {
                StartCoroutine(IniciarSiguienteOleada());
            }
            else
            {
                AlCompletarTodasOleadas?.Invoke();
            }
        }
    }

    void SpawnearMiniJefe()
    {
        Debug.Log("¡Mini Jefe apareciendo!");
        
        if (prefabMiniJefe != null)
        {
            Vector2 posicionSpawneo = puntoSpawneoJefe != null ? 
                puntoSpawneoJefe.position : ObtenerPosicionSpawneoAleatoria();
            
            GameObject jefe = Instantiate(prefabMiniJefe, posicionSpawneo, Quaternion.identity);
            
            // Agregar a la lista de enemigos activos
            enemigosActivos.Add(jefe);
            
            // Configurar referencia al SistemaOleadas para el jefe (EnemyPareja)
            EnemyPareja scriptJefe = jefe.GetComponent<EnemyPareja>();
            if (scriptJefe != null)
            {
                // Si tu EnemyPareja tiene método para configurar
                ConfigurarJefe(scriptJefe, jefe);
            }
            
            AlAparecerJefe?.Invoke();
        }
    }

    // Método para configurar el jefe
    void ConfigurarJefe(EnemyPareja jefeScript, GameObject jefeObj)
    {
        // Similar a ConfigurarEnemigo pero para el jefe
        EnemyOleadaManager oleadaManager = jefeObj.GetComponent<EnemyOleadaManager>();
        if (oleadaManager == null)
        {
            oleadaManager = jefeObj.AddComponent<EnemyOleadaManager>();
        }
        oleadaManager.Configurar(this, jefeObj);
    }

    // Llamado cuando el mini jefe es derrotado
    public void JefeDerrotado()
    {
        Debug.Log("¡Mini Jefe derrotado!");
        
        // Pasar a la siguiente oleada
        oleadaActual++;
        if (oleadaActual < oleadas.Count)
        {
            StartCoroutine(IniciarSiguienteOleada());
        }
        else
        {
            AlCompletarTodasOleadas?.Invoke();
        }
    }

    // Método para que los enemigos se remuevan de la lista cuando son destruidos
    public void RemoverEnemigo(GameObject enemigo)
    {
        if (enemigosActivos.Contains(enemigo))
        {
            enemigosActivos.Remove(enemigo);
        }
    }

    // Métodos para UI
    public int ObtenerNumeroOleadaActual()
    {
        return oleadaActual + 1;
    }

    public int ObtenerTotalOleadas()
    {
        return oleadas.Count;
    }

    public int ObtenerEnemigosVivos()
    {
        return enemigosVivos;
    }

    public int ObtenerTotalEnemigosEnOleada()
    {
        if (oleadaActual < oleadas.Count)
            return oleadas[oleadaActual].cuotaOleada;
        return 0;
    }

    public bool EstaSpawneando()
    {
        return estaSpawneando;
    }

    public bool TodasOleadasCompletadas()
    {
        return oleadaActual >= oleadas.Count;
    }

    // Método para detener el spawneo (del EnemySpawner original)
    public void DetenerSpawneo()
    {
        estaSpawneando = false;
        StopAllCoroutines();
    }

    void OnDestroy() => DetenerSpawneo();
    void OnDisable() => DetenerSpawneo();

    // Visualización de áreas de spawneo en el Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        
        // Dibujar áreas de spawneo por defecto si no hay puntos definidos
        if (puntosSpawneo == null || puntosSpawneo.Count == 0)
        {
            Gizmos.DrawLine(new Vector3(-15f, -2.5f, 0), new Vector3(-11f, -2.5f, 0));
            Gizmos.DrawLine(new Vector3(11f, -2.5f, 0), new Vector3(14.4f, -2.5f, 0));
        }
        else
        {
            // Dibujar puntos de spawneo definidos
            foreach (Transform punto in puntosSpawneo)
            {
                if (punto != null)
                {
                    Gizmos.DrawWireSphere(punto.position, 0.5f);
                }
            }
        }
        
        // Dibujar punto de spawneo del jefe
        if (puntoSpawneoJefe != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(puntoSpawneoJefe.position, 0.7f);
        }
    }
}

// Componente auxiliar para manejar la conexión con el sistema de oleadas
public class EnemyOleadaManager : MonoBehaviour
{
    private SistemaOleadas sistemaOleadas;
    private GameObject enemigo;

    public void Configurar(SistemaOleadas sistema, GameObject enemyObj)
    {
        this.sistemaOleadas = sistema;
        this.enemigo = enemyObj;
    }

    void OnDestroy()
    {
        // Cuando el enemigo es destruido, notificar al sistema de oleadas
        if (sistemaOleadas != null)
        {
            sistemaOleadas.EnemigoEliminado(enemigo);
        }
    }
}