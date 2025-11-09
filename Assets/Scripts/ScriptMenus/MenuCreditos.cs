using UnityEngine;
using UnityEngine.UI;

public class MenuCreditos : IUPanel
{
    [SerializeField] private Button btVolver;
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

    }

    public override void Ocultar()
    {
        gameObject.SetActive(false);
    }
}
