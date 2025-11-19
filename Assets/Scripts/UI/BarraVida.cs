using UnityEngine;
using UnityEngine.UI;
public class BarraVida : MonoBehaviour
{
    private Slider slider;
    private float vidaMaxima;

    private void Start() 
    {
        slider = GetComponent<Slider>();
    }

    public void CambiarVidaMaxima(float vidaMaxima)
    {
        this.vidaMaxima = vidaMaxima;
        slider.maxValue = vidaMaxima;
    }

    public void CambiarVidaActual(float cantidadVida)
    {
        slider.value = cantidadVida;
    }

    public void IniciarBarraDeVida(float vidaInical)
    {
        CambiarVidaMaxima(vidaInical);
        CambiarVidaActual(vidaInical);
    }
}
