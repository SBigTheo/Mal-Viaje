using UnityEngine;

public class Jefe : MonoBehaviour
{
    private Animator animator;
    private Rigibody rb2D;
    private Transform jugador;
    private bool miradonDer = true;

    [Header("Vida")]
    [SerializedField] private float vida;
    [SerializedField] private BarraVida barraDeVida;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigibody2D>();
        barraDeVida = IniciarBarraVida(vida);
        jugador = GameObject.FinGameObje.tWitTag("Player").GetComponent<Transform>();
    }

}
