using UnityEngine;
using UnityEngine.UI;
public class BarraVida : MonoBehaviour
{
    private Slider slider;
    private float vidaMaxima;

    private void Awake() 
    {
        slider = GetComponent<Slider>();

        if (slider == null)
        {
            Debug.LogError("NO esta el componenete slider" +  gameObject.name);
        }
    }

    public void CambiarVidaMaxima(float vidaMaxima)
    {
        if (slider == null)
        {
            Debug.LogError("Slider es nulo en vida maxima metodo");
            return;
        }
        this.vidaMaxima = vidaMaxima;
        slider.maxValue = vidaMaxima;
    }

    public void CambiarVidaActual(float cantidadVida)
    {
        if (slider == null)
        {
            Debug.LogError("Slider es nulo en vida actual metodo");
            return;
        }
        slider.value = cantidadVida;
    }

    public void IniciarBarraDeVida(float vidaInical)
    {
        if(slider == null)
        {
            slider = GetComponent<Slider>();
            if (slider == null)
            {
            Debug.LogError("NO se inicia la barra");
            return;
            }
        }
        CambiarVidaMaxima(vidaInical);
        CambiarVidaActual(vidaInical);
    }
}
