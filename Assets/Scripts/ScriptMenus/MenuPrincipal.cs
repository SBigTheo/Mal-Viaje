using UnityEngine;
using UnityEngine.UI;

public class MenuPrincipal : IUPanel
{
    [SerializeField] private Button btJugar;
    [SerializeField] private Button btNiveles;
    [SerializeField] private Button btCreditos;
    [SerializeField] private Button btSalir;
    [SerializeField] private GestorIU gestor;

    public override void Mostrar()
    {
        gameObject.SetActive(true);

        if (gestor == null)
        {
            Debug.LogError("EL gestor IU no se asigno bien");
            return;
        }

        btJugar.onClick.RemoveAllListeners(); 
        btJugar.onClick.AddListener(() =>{ 
            PlayerPrefs.SetString("EscenaDestino", "Nivel1");
            PlayerPrefs.Save();
            UnityEngine.SceneManagement.SceneManager.LoadScene("SlidesHistoria");});

        btNiveles.onClick.RemoveAllListeners();
        btNiveles.onClick.AddListener(() => gestor.MostrarPaneles(1));

        btCreditos.onClick.RemoveAllListeners(); 
        btCreditos.onClick.AddListener(() => gestor.MostrarPaneles(2));

        btSalir.onClick.RemoveAllListeners();
        btSalir.onClick.AddListener(() => gestor.Salir());
    }

    public override void Ocultar()
    {
        gameObject.SetActive(false);
    }
}