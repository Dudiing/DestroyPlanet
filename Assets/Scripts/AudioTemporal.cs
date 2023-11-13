using UnityEngine;

public class AudioTemporal : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Verifica si el AudioSource existe y tiene un clip asignado
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            Destroy(gameObject, audioSource.clip.length); // Destruye el objeto después de que el clip haya terminado
        }
        else
        {
            // Si no hay AudioSource o clip, destruye el objeto para evitar que quede en la escena
            Destroy(gameObject);
        }
    }

    public void SetAudioClip(AudioClip clip)
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = clip;
    }
}
