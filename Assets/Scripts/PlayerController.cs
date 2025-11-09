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
    private Vector2 lastMovementDirection = Vector2.right;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        float velocidadX = Input.GetAxis("Horizontal") * velocidad;
        rb.linearVelocity = new Vector2(velocidadX, rb.linearVelocity.y);

        if (velocidadX != 0)
        {
            lastMovementDirection = velocidadX > 0 ? Vector2.right : Vector2.left;
        }

        enSuelo = Physics2D.Raycast(transform.position, Vector2.down, longitud, capaSuelo);

        if (Input.GetKeyDown(KeyCode.W) && enSuelo)
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
    }

    public Vector2 GetLastMovementDirection()
    {
        return lastMovementDirection;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitud);
    }
}