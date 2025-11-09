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

        btJugar.onClick.RemoveAllListeners(); // Corregido: RemoveAllListeners
        btJugar.onClick.AddListener(() => gestor.MostrarPaneles(1));

        btNiveles.onClick.RemoveAllListeners(); // Corregido: RemoveAllListeners
        btNiveles.onClick.AddListener(() => gestor.MostrarPaneles(2));

        btCreditos.onClick.RemoveAllListeners(); // Corregido: RemoveAllListeners
        btCreditos.onClick.AddListener(() => gestor.MostrarPaneles(3));

        btSalir.onClick.RemoveAllListeners(); // Corregido: RemoveAllListeners
        btSalir.onClick.AddListener(() => gestor.Salir());
    }

    public override void Ocultar()
    {
        gameObject.SetActive(false);
    }
}