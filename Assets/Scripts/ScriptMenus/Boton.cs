using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Script para efectos hover
public class Boton : MonoBehaviour
{
    [Header("Imagenes")]
    public Sprite mariImagen;
    public Sprite puchoImagen;

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
                botonImagen.sprite = puchoImagen;
            }
        }
    }

    public void CursorYaNoPasa()
    {
        esApretado = false;
        botonImagen.sprite = mariImagen;
    }

    void estaDesactivado()
    {
        esApretado = false;
        if(botonImagen != null && mariImagen != null)
        {
            botonImagen.sprite = mariImagen;
        }
    }

    void estaActivado()
    {
        if (botonImagen != null && mariImagen != null && !esApretado)
        {
            botonImagen.sprite = mariImagen;
        }
    }
}
