using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 6f;
    public float longitud = 1f;
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
        // animator.SetBool("EnSuelo", enSuelo);
        // animator.SetFloat("VelocidadY", rb.linearVelocity.y);

        // Salto
        if (Input.GetKeyDown(KeyCode.W) && enSuelo)
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
    
    public Vector2 GetLastMovementDirection()
{
    return new Vector2(lastHorizontalDirection, 0f);
}

    public float GetLastHorizontalDirection()
    {
        return lastHorizontalDirection;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitud);
    }
}