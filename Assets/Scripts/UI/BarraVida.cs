using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    // private PlayerHealth playerHealth; 
    private float maxHealth;
    private float currentHealth;

    internal void IniciarBarraVida(float vidaMaxima)
    {
        // throw new NotImplementedException();

        maxHealth = vidaMaxima;
        currentHealth = vidaMaxima;
        ActualizarBarra();
    }

    internal void CambiarVidaActual(float vidaActual)
    {
        // throw new NotImplementedException();

        currentHealth = Mathf.Clamp(vidaActual, 0, maxHealth);
        ActualizarBarra();
    }

    private void ActualizarBarra()
    {
        if( rellenoBarraVida != null)
        {
            rellenoBarraVida.fillAmount = currentHealth/maxHealth;
        }
    }

    public void ConfigurarBarra( float vidaMaxima, float vidaActual)
    {
        maxHealth = vidaMaxima;
        currentHealth = Mathf.Clamp(vidaActual, 0, vidaMaxima);
        ActualizarBarra();
    }

    // private void Start()
    // {
    //    playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    //     maxHealth = playerHealth.currentHealth;
    // }

    // void Update()
    // {
    //     rellenoBarraVida.fillAmount = playerHealth.currentHealth / maxHealth;
    // }
}
