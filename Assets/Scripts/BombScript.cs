using System.Collections;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private GameManager gameManager;
    private Renderer bombRenderer;
    private float explosionTime = 4f;
    private float blinkDuration = 1f;
    private Color originalColor;
    private Coroutine bombExplosionCoroutine;

    public GameObject explosionPrefab;
    public GameObject audioEffectPrefab;
    public AudioClip clickAudioClip;
    public AudioClip explosionAudioClip;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        bombRenderer = GetComponent<Renderer>();
        if (bombRenderer)
        {
            originalColor = bombRenderer.material.color;
        }
        bombExplosionCoroutine = StartCoroutine(BombExplosion());
    }

    IEnumerator BombExplosion()
    {
        yield return new WaitForSeconds(explosionTime - blinkDuration);
        
        float elapsedTime = 0;
        while (elapsedTime < blinkDuration)
        {
            float lerpTime = Mathf.PingPong(elapsedTime * 4, 1);
            bombRenderer.material.color = Color.Lerp(originalColor, Color.red, lerpTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        TriggerExplosion();
        if (gameManager != null)
        {
            gameManager.TakeDamage();
        }
        Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.gameObject == this.gameObject)
            {
                OnBombClick();
            }
        }
    }

    private void OnBombClick()
    {
        StopCoroutine(bombExplosionCoroutine);
        bombRenderer.material.color = originalColor;
        
        if (gameManager != null)
        {
            gameManager.AddScore();
        }
        PlayAudioEffect(clickAudioClip);
        Destroy(gameObject);
    }

    public void StopExplosion()
    {
        if (bombExplosionCoroutine != null)
        {
            StopCoroutine(bombExplosionCoroutine);
            bombRenderer.material.color = originalColor;
            Destroy(gameObject);
        }
    }

    private void TriggerExplosion()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        PlayAudioEffect(explosionAudioClip);
    }

    private void PlayAudioEffect(AudioClip clip)
    {
        if (audioEffectPrefab != null && clip != null)
        {
            GameObject audioEffect = Instantiate(audioEffectPrefab, transform.position, Quaternion.identity);
            AudioSource audioSource = audioEffect.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();

            Destroy(audioEffect, clip.length);
        }
    }
}
