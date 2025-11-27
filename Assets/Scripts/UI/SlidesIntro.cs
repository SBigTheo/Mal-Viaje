using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SlidesIntro : MonoBehaviour
{
    [SerializeField] private Image imagenSlide;
    [SerializeField] private List<Sprite> slides;
    [SerializeField] private string escenaMenu = "MenuPrincipal";
    
    private int slideActual = 0;
    private bool puedeAvanzar = true;
    private bool introCompletada = false;
    
    void Start()
    {
        
        
        // if (PlayerPrefs.GetInt("IntroCompletada", 0) == 1)
        // {
        //     Debug.Log("Intro ya fue vista anteriormente, cargando menú...");
        //     CargarMenuPrincipal();
        //     return;
        // }

        InicializarSlides();
    }
    
    void InicializarSlides()
    {
        if (imagenSlide == null)
        {
            Debug.LogError("Image no asignado en el inspector");
            CargarMenuPrincipal();
            return;
        }

        if (slides == null || slides.Count == 0)
        {
            Debug.LogError("Lista de slides vacía o nula");
            CargarMenuPrincipal();
            return;
        }

        
        foreach (var slide in slides)
        {
            if (slide == null)
            {
                Debug.LogError("Uno o más slides son nulos");
                CargarMenuPrincipal();
                return;
            }
        }

        Debug.Log($"Slides cargados correctamente: {slides.Count} slides");
        MostrarSlideActual();
        StartCoroutine(DetectarInput());
    }
    
    void MostrarSlideActual()
    {
        if (slideActual >= 0 && slideActual < slides.Count && slides[slideActual] != null)
        {
            imagenSlide.sprite = slides[slideActual];
            StartCoroutine(CooldownAvance());
            
            Debug.Log($"Mostrando slide {slideActual + 1}/{slides.Count}");
        }
        else
        {
            Debug.LogError($"Índice de slide inválido: {slideActual}");
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
    
    private IEnumerator CooldownAvance()
    {
        puedeAvanzar = false;
        yield return new WaitForSeconds(0.3f);
        puedeAvanzar = true;
    }
    
    private IEnumerator DetectarInput()
    {
        while (!introCompletada)
        {
            // Click izquierdo del mouse
            if (Input.GetMouseButtonDown(0))
            {
                AvanzarSlide();
            }
            
            // Teclas para avanzar
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                AvanzarSlide();
            }
            
            // Teclas para retroceder
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Backspace))
            {
                RetrocederSlide();
            }
            
            // Saltar intro
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
            Debug.Log("Saltando todos los slides");
            CompletarIntro();
        }
    }
    
    private void CompletarIntro()
    {
        if (introCompletada) return;
        
        introCompletada = true;
        StopAllCoroutines();
        
        // Marcar como completado
        PlayerPrefs.SetInt("IntroCompletada", 1);
        PlayerPrefs.Save();
        
        Debug.Log("Intro completada, cargando menú...");
        CargarMenuPrincipal();
    }
    
    private void CargarMenuPrincipal()
    {
        SceneManager.LoadScene(escenaMenu);
    }
}