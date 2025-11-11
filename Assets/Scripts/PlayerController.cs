using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 6f;
    public float longitud = 1f;
    public LayerMask capaSuelo;

    private bool enSuelo;
    private Rigidbody2D rb;
    private Animator animator;
    private float lastHorizontalDirection = 1f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
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