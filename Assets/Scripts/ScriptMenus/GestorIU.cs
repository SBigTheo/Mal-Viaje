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


    void Update()
    {
        
    }

    public void MostrarPaneles(int indice){
        if(indice < 0 || indice >= paneles.length){
            Debug.LogError("Error el indice es malo....");
            return;
        }

        if(panelAct != null){
            panelAct.Ocultar();
        }

        paneles[indice].Mostrar();
        panelAct = paneles[indice];
    }

    public void OcultarPaneles(){
        for(int i = 0; i < paneles.length; i++){
            paneles[i].Ocultar();
        }
    }

    public void Salir(){
        #if UNITy_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
