using UnityEngine;
using UnityEngine.UI;

// Script para efectos hover
public class Boton : MonoBehaviour
{
    [Header("Imagenes")]

    public Sprite mariImagen;
    public Sprite puchoImagen;

    private Image botonImagen;
    private Sprite originalImagen;

    void Start() 
    {
        botonImagen = GetComponent<Image>();
        originalImagen = botonImagen.sprite;

        if(mariImagen == null)
        {
            mariImagen = originalImagen;
        }
    }

    public void CursorPasa()
    {
        if (puchoImagen != null)
        {
            botonImagen.sprite = puchoImagen;
        }
    }

    public void CursorYaNoPasa()
    {
        botonImagen.sprite = mariImagen;
    }
}
