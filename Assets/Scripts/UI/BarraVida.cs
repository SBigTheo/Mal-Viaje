using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;
    private PlayerHealth playerHealth; 
    private float maxHealth;

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
