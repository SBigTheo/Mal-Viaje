using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EfectoDanoImagen : MonoBehaviour
{
    public float duracionEfecto = 0.3f;
    public float intensidadMaxima = 0.7f;
    public Color colorEfecto = Color.red;

    private Image imagen;
    private Color colorOriginal;
    private Coroutine efectoCoroutine;

    private void Start()
    {
        imagen = GetComponent<Image>();

        if (imagen != null)
        {
            colorOriginal = imagen.color;
        }
    }

    public void ActivarEfecto()
    {
        if (imagen == null) return;

        if (efectoCoroutine != null)
        {
            StopCoroutine(efectoCoroutine);
        }

        efectoCoroutine = StartCoroutine(EfectoCoroutine());
    }

    private IEnumerator EfectoCoroutine()
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionEfecto)
        {
            tiempoTranscurrido += Time.deltaTime;
            float progreso = tiempoTranscurrido / duracionEfecto;

            Color colorInterpolado = Color.Lerp(
                new Color(colorEfecto.r, colorEfecto.g, colorEfecto.b, intensidadMaxima),
                colorOriginal,
                progreso
            );

            imagen.color = colorInterpolado;

            yield return null;
        }

        imagen.color = colorOriginal;
        efectoCoroutine = null;
    }

    public void ActivarEfectoPersonalizado(Color colorPersonalizado, float duracionPersonalizada, float intensidadPersonalizada)
    {
        if (imagen == null) return;

        if (efectoCoroutine != null)
        {
            StopCoroutine(efectoCoroutine);
        }

        efectoCoroutine = StartCoroutine(EfectoPersonalizadoCoroutine(colorPersonalizado, duracionPersonalizada, intensidadPersonalizada));
    }

    private IEnumerator EfectoPersonalizadoCoroutine(Color colorPersonalizado, float duracionPersonalizada, float intensidadPersonalizada)
    {
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionPersonalizada)
        {
            tiempoTranscurrido += Time.deltaTime;
            float progreso = tiempoTranscurrido / duracionPersonalizada;

            Color colorInterpolado = Color.Lerp(
                new Color(colorPersonalizado.r, colorPersonalizado.g, colorPersonalizado.b, intensidadPersonalizada),
                colorOriginal,
                progreso
            );

            imagen.color = colorInterpolado;

            yield return null;
        }

        imagen.color = colorOriginal;
        efectoCoroutine = null;
    }

    public void DetenerEfecto()
    {
        if (efectoCoroutine != null)
        {
            StopCoroutine(efectoCoroutine);
            efectoCoroutine = null;
        }

        if (imagen != null)
        {
            imagen.color = colorOriginal;
        }
    }
}