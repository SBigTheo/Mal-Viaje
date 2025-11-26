using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class ControladorSlides : MonoBehaviour
{
    [SerializeField] private Image imagenSlide;
    [SerializeField] private List<Sprite> slides;
    [SerializeField] private string escenaDestino = "Nivel1";

    private int slideActual ;
    private bool puedenAvanzar = false;
    
    void Start()
    {
        if (PlayerPrefs.HasKey("EscenaDestino"))
        {
            escenaDestino = PlayerPrefs.GetString("EscenaDestino");
        }

        MostrarSlideActual();
        StartCoroutine(DetectarInput());
    }

    void MostrarSlideActual()
    {
        if (slides.Count > slides[slideActual >= 0 && slideActual < slides.Count])
        {
            imagenSlide.Sprite = slides[slideActual];
            StartCoroutine(CooldownAvance());
        }
    }

    public void AvanzarSlide()
    {
        if(!puedenAvanzar) return;

        if (slideActual < slides.Count - 1)
        {
            slideActual++;
            MostrarSlideActual();
        }
    }

    public void RetrocederSlide()
    {
        if (!puedenAvanzar || slideActual <= 0) return;
        
            slideActual--;
            MostrarSlideActual();
    }

    private System.Collections.IEnumerator CooldownAvance()
    {
        puedenAvanzar = false;
        yield return new WaitForSeconds(0.3f);
        puedenAvanzar = true;
    }

    public void SaltarTodosSlides()
    {
        CargarEscenarioDestino();
    }

    private void CargarEscenarioDestino()
    {
        SceneManager.LoadScene(escenaDestino);
    }
}
