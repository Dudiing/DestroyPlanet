using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float lifetime = 2.0f; // Tiempo en segundos antes de destruir el objeto

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
