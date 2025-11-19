using UnityEngine;
using UnityEngine.UI;
public class BarraVida : MonoBehaviour
{
    private Slider slider;

    private void Start() 
    {
        slider = GetComponent<Slider>();
    }

    public void CambiarVidaMaxima(float vidaMaxima)
    {
        slider.maxValue = vidaMaxima;
    }

    public void CambiarVidaActual( int cantidadVida)
    {
        slider.value = cantidadVida;
    }

    public void IniciarBarraDeVida()
    {
        CambiarVidaMaxima(cantidadVida);
        CambiarVidaActual(cantidadVida);
    }
}
