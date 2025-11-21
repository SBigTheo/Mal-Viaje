using UnityEngine;
using UnityEngine.UI;
public class BarraDeVidaUI : MonoBehaviour
{
    [SerializeField] private Slider sliderBarraDeVida;

    public void IniciarBarraDeVidaPlayer(int maxHealth, int currentHealth)
    {
        sliderBarraDeVida.maxValue = maxHealth;
        sliderBarraDeVida.value = currentHealth;
    }

    public void CambiarBarraDeVida(int currentHealth)
    {
        sliderBarraDeVida.value = currentHealth;
    }
}
