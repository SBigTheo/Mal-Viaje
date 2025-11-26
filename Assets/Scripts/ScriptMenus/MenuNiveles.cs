using UnityEngine;
using UnityEngine.UI;

public class MenuNiveles : IUPanel
{
    [SerializeField] private Button btVolver;
    [SerializeField] private Button btNivel1;
    [SerializeField] private Button btNivel2;
    [SerializeField] private Button btNivel3;
    [SerializeField] private GestorIU gestor;

    public override void Mostrar()
    {
        gameObject.SetActive(true);

        if (gestor == null)
        {
            Debug.LogError("EL gestor IU no se asigno bien" +gestor.name);
            return;
        }

        btVolver.onClick.RemoveAllListeners();
        btVolver.onClick.AddListener(() => gestor.MostrarPaneles(0));

        btNivel1.onClick.RemoveAllListeners(); 
        btNivel1.onClick.AddListener(() =>{  
            PlayerPrefs.SetString("EscenaDestino", "Nivel1");
            PlayerPrefs.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene("SlidesHistoria");;});

        btNivel2.onClick.RemoveAllListeners(); 
        btNivel2.onClick.AddListener(() =>{ UnityEngine.SceneManagement.SceneManager.LoadScene("Nivel2");});
        
        btNivel3.onClick.RemoveAllListeners(); 
        btNivel3.onClick.AddListener(() =>{ UnityEngine.SceneManagement.SceneManager.LoadScene("Nivel3");});

    }

    public override void Ocultar()
    {
        gameObject.SetActive(false);
    }
}
