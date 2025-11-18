using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Script para efectos hover
public class Boton : MonoBehaviour
{
    [Header("Imagenes")]

    public Sprite mariImagen;
    public Sprite puchoImagen;

    [Header("Transicion")]
    public float velocidad = 5f;

    private Image botonImagen;

    private bool esApretado = false;
    // private Sprite originalImagen;

    void Start() 
    {
        botonImagen = GetComponent<Image>();
        // originalImagen = botonImagen.sprite;

        if(mariImagen == null)
        {
            mariImagen = botonImagen.sprite;
        }
    }

    public void CursorPasa()
    {
        if (puchoImagen != null)
        {
            esApretado = true;
            if(puchoImagen != null)
            {
                StopAllCoroutines();
                StartCoroutine(CambiarDeImagen(puchoImagen));
            }
            // botonImagen.sprite = puchoImagen;
        }
    }

    public void CursorYaNoPasa()
    {
        esApretado = false;
        StopAllCoroutines();
        StartCoroutine(CambiarDeImagen(mariImagen));
        // botonImagen.sprite = mariImagen;
    }

    IEnumerator CambiarDeImagen(Sprite obtenerImage)
    {
        yield return new WaitForSeconds(0.05f);
        if ((esApretado && obtenerImage == puchoImagen) || (!esApretado && obtenerImage == mariImagen)) 
        {
            botonImagen.sprite = obtenerImage;
        }
    }
}
