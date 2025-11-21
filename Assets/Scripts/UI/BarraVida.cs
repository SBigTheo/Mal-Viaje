using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private PlayerHealth playerHealth; 
    private float maxHealth;

    internal void CambiarVidaActual(float vida)
    {
        throw new NotImplementedException();
    }

    internal void IniciarBarraVida(float vida)
    {
        throw new NotImplementedException();
    }

    private void Start()
    {
       playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        maxHealth = playerHealth.currentHealth;
    }

    void Update()
    {
        rellenoBarraVida.fillAmount = playerHealth.currentHealth / maxHealth;
    }
}
