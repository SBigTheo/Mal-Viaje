using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 6f;
    public float longitud = 1f;
    private bool estaEnSuelo = false;
    public LayerMask capaSuelo;

    [Header("Sistema de Ataque")]
    public List<Ataque> ataquesDisponibles = new List<Ataque>();
    public bool debugMode = false;

    private bool enSuelo;
    private Rigidbody2D rb;
    private Animator animator;
    private float lastHorizontalDirection = 1f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        InicializarAtaques();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        Movimiento();
        Salto();
        ProcesarAtaques();
    }
    void InicializarAtaques()
    {
        // Obtiene todos los componentes de ataque
        Ataque[] ataquesEncontrados = GetComponents<Ataque>();
        ataquesDisponibles.AddRange(ataquesEncontrados);

        Debug.Log($"Se encontraron {ataquesDisponibles.Count} ataques en el jugador");
    }

    void Movimiento()
    {
        if (Time.timeScale == 0f) return;

        float velocidadX = Input.GetAxis("Horizontal") * velocidad;

        //Movimiento en el area del suelo
        if (estaEnSuelo)
        {
            float velocidadY = Input.GetAxis("Vertical") * velocidad;
            rb.linearVelocity = new Vector2(velocidadX, velocidadY);

            animator.SetFloat("Horizontal", Mathf.Abs(velocidadX));

            if (velocidadX != 0)
            {
                lastHorizontalDirection = Mathf.Sign(velocidadX);
                transform.localScale = new Vector3(lastHorizontalDirection, 1, 1);
            }
            return;
        }

        //MOvimiento que ya teniamos 
        rb.linearVelocity = new Vector2(velocidadX, rb.linearVelocity.y);

        // Animación de correr
        animator.SetFloat("Horizontal", Mathf.Abs(velocidadX));

        // Cambia la dirección del sprite
        if (velocidadX != 0)
        {
            lastHorizontalDirection = Mathf.Sign(velocidadX);
            transform.localScale = new Vector3(lastHorizontalDirection, 1f, 1f);
        }
    }

    void Salto()
    {
        // Detección del suelo
        enSuelo = Physics2D.Raycast(transform.position, Vector2.down, longitud, capaSuelo);

        // // Animación de salto/caída
        animator.SetBool("EnSuelo", enSuelo);
        animator.SetFloat("VelocidadY", rb.linearVelocity.y);

        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
    }

    void ProcesarAtaques()
    {
        foreach (Ataque ataque in ataquesDisponibles)
        {
            if (ataque == null) continue;
            if (Input.GetKeyDown(ataque.teclaAtaque) && !ataque.EstaEnCooldown())
            {
                ataque.EjecutarAtaque();
                break;
            }
        }
    }

    public void DesbloquearAtaque(System.Type tipoAtaque)
    {
        Ataque ataqueExistente = GetComponent(tipoAtaque) as Ataque;
        if (ataqueExistente == null)
        {
            gameObject.AddComponent(tipoAtaque);
            InicializarAtaques();
            Debug.Log($"Ataque {tipoAtaque.Name} estas usando");
        }
    }

    void ActivarAtaque(KeyCode tecla, bool activar)
    {
        foreach (Ataque ataque in ataquesDisponibles)
        {
            if (ataque != null && ataque.teclaAtaque == tecla)
            {
                ataque.enabled = activar;
                break;
            }
        }
    }

    public List<Ataque> GetAtaquesActivos()
    {
        List<Ataque> ataquesActivos = new List<Ataque>();
        foreach (Ataque ataque in ataquesDisponibles)
        {
            if (ataque != null && ataque.enabled)
            {
                ataquesActivos.Add(ataque);
            }
        }
        return ataquesActivos;
    }
    public Vector2 GetLastMovementDirection()
{
    return new Vector2(lastHorizontalDirection, 0f);
}

    public float GetLastHorizontalDirection()
    {
        return lastHorizontalDirection;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("suelo"))
        {
            estaEnSuelo = true;
            rb.gravityScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("suelo"))
        {
            estaEnSuelo = false;
            rb.gravityScale = 1;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitud);
    }
}