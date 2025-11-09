using UnityEngine;

public class GestorIU : MonoBehaviour
{
    [Header("Colocar los panales en orden acá")]
    public IUPanel[] paneles;
    private IUPanel panelAct;

    void Start()
    {
        OcultarPaneles();
        MostrarPaneles(0);
    }

    public void MostrarPaneles(int indice)
    {
        if (indice < 0 || indice >= paneles.Length)
        {
            Debug.LogError("Error el indice es malo....");
            return;
        }

        if (panelAct != null)
        {
            panelAct.Ocultar();
        }

        paneles[indice].Mostrar();
        panelAct = paneles[indice];
    }

    public void OcultarPaneles()
    {
        for (int i = 0; i < paneles.Length; i++)
        {
            paneles[i].Ocultar();
        }
    }

    public void Salir()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}