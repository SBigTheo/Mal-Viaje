using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("Audio Clip Escene")]
    public AudioClip background;

    [Header("Audio Clip Player")]
    public AudioClip piña;
    public AudioClip patada;
    public AudioClip golpeEspecial;
    public AudioClip muertePlayer;
    public AudioClip caminar;
    public AudioClip dañoRecibidoPorEnemigo;

    [Header("Audio Clip Enemy")]
    public AudioClip muerteEnemigo;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}