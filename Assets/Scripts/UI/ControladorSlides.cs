using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ControladorSlidesMouse : MonoBehaviour
{
    [SerializeField] private Image imagenSlide;
    [SerializeField] private List<Sprite> slides;
    [SerializeField] private string escenaMenu = "Menu"; // Cambiado a menú principal
    
    private int slideActual = 0;
    private bool puedeAvanzar = true;
    private bool introCompletada = false;
    
    void Start()
    {
        // Verificar si ya se completó la intro anteriormente
        if (PlayerPrefs.GetInt("IntroCompletada", 0) == 1)
        {
            // Si ya se vio la intro, ir directamente al menú
            CargarMenuPrincipal();
            return;
        }

        // Configurar para mostrar los slides
        if (slides.Count > 0)
        {
            MostrarSlideActual();
            StartCoroutine(DetectarInput());
        }
        else
        {
            // Si no hay slides, ir al menú directamente
            CargarMenuPrincipal();
        }
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
        if (!puedeAvanzar || introCompletada) return;
        
        if (slideActual < slides.Count - 1)
        {
            slideActual++;
            MostrarSlideActual();
        }
        else
        {
            CompletarIntro();
        }
    }
    
    public void RetrocederSlide()
    {
        if (!puedeAvanzar || slideActual <= 0 || introCompletada) return;
        
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
        while (!introCompletada)
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
        if (!introCompletada)
        {
            CompletarIntro();
        }
    }
    
    private void CompletarIntro()
    {
        introCompletada = true;
        
        // Marcar que la intro ya fue vista
        PlayerPrefs.SetInt("IntroCompletada", 1);
        PlayerPrefs.Save();
        
        // Cargar el menú principal
        CargarMenuPrincipal();
    }
    
    private void CargarMenuPrincipal()
    {
        SceneManager.LoadScene(escenaMenu);
    }
}