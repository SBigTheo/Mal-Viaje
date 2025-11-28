using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ControladorSlides : MonoBehaviour
{
    [SerializeField] private Image imagenSlide;
    [SerializeField] private List<Sprite> slides;
    [SerializeField] private string escenaDestino = "Nivel1";
    
    private int slideActual = 0;
    private bool puedeAvanzar = true;
    
    void Start()
    {
        string escenaActual = SceneManager.GetActiveScene().name;
        
        if (escenaActual.Contains("SlideNivel1"))
        escenaDestino = "Nivel1";
        else if (escenaActual.Contains("SlideNivel2"))
        escenaDestino = "Nivel2";
        else if (escenaActual.Contains("SlideNivel3"))
        escenaDestino = "Nivel3";
        
        MostrarSlideActual();
        StartCoroutine(DetectarInput());
        }
    
    void MostrarSlideActual()
    {
        if (slides.Count > 0 && slideActual >= 0 && slideActual < slides.Count)
        {
            imagenSlide.sprite = slides[slideActual];
            StartCoroutine(CooldownAvance());
        }
    }
    
    public void AvanzarSlide()
    {
        if (!puedeAvanzar) return;
        
        if (slideActual < slides.Count - 1)
        {
            slideActual++;
            MostrarSlideActual();
        }
        else
        {
            CargarEscenaDestino();
        }
    }
    
    public void RetrocederSlide()
    {
        if (!puedeAvanzar || slideActual <= 0) return;
        
        slideActual--;
        MostrarSlideActual();
    }
    
    private System.Collections.IEnumerator CooldownAvance()
    {
        puedeAvanzar = false;
        yield return new WaitForSeconds(0.3f);
        puedeAvanzar = true;
    }
    
    private System.Collections.IEnumerator DetectarInput()
    {
        while (true)
        {
            // Click del botón izquierdo
            if (Input.GetMouseButtonDown(0))
            {
                AvanzarSlide();
            }
            
            // Teclado - Espacio o Enter para avanzar
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
            {
                AvanzarSlide();
            }
            
            // Teclado - Flecha izquierda o Backspace para retroceder
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Backspace))
            {
                RetrocederSlide();
            }
            
            // Teclado - Escape para saltar todos los slides
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SaltarTodosSlides();
            }
            
            yield return null;
        }
    }
    
    public void SaltarTodosSlides()
    {
        CargarEscenaDestino();
    }
    
    private void CargarEscenaDestino()
    {
        SceneManager.LoadScene(escenaDestino);
    }
}