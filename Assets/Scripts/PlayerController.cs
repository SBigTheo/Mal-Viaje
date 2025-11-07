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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float velocidadX = Input.GetAxis("Horizontal") * velocidad;
        rb.linearVelocity = new Vector2(velocidadX, rb.linearVelocity.y);

        enSuelo = Physics2D.Raycast(transform.position, Vector2.down, longitud, capaSuelo);

        if (Input.GetKeyDown(KeyCode.W) && enSuelo)
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitud);
    }
}