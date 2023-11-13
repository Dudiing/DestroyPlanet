using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject prefabBomb;
    public Vector3 minSpawnValues = new Vector3(-10, 0, -10);
    public Vector3 maxSpawnValues = new Vector3(10, 0, 10);

    [SerializeField] private float spawnInterval = 4f; // Intervalo inicial para generar bombas
    [SerializeField] private float minSpawnInterval = 1f; // Intervalo mínimo entre generaciones de bombas
    [SerializeField] private float intervalReduction = 0.1f; // Cuánto se reduce el intervalo por generación
    [SerializeField] private int reductionFrequency = 10; // Frecuencia de reducción del intervalo

    private Coroutine spawnRoutine;
    private bool spawningActive = false;
    private int spawnCount = 0; // Contador de generaciones de bombas

    public void StartSpawning()
    {
        if (!spawningActive)
        {
            spawningActive = true;
            spawnRoutine = StartCoroutine(SpawnBombRoutine());
        }
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            spawningActive = false;
            StopCoroutine(spawnRoutine);
        }
    }

    IEnumerator SpawnBombRoutine()
    {
        while (spawningActive)
        {
            SpawnBomb();
            yield return new WaitForSeconds(spawnInterval);
            AdjustSpawnInterval();
        }
    }

    void SpawnBomb()
    {
        Vector3 randPosition = new Vector3(
            Random.Range(minSpawnValues.x, maxSpawnValues.x),
            Random.Range(minSpawnValues.y, maxSpawnValues.y),
            Random.Range(minSpawnValues.z, maxSpawnValues.z)
        );

        Instantiate(prefabBomb, randPosition, Quaternion.identity);
        spawnCount++;
    }

    // Ajusta el intervalo de generación de bombas
    private void AdjustSpawnInterval()
    {
        if (spawnCount % reductionFrequency == 0 && spawnInterval > minSpawnInterval)
        {
            spawnInterval -= intervalReduction;
        }
    }
}
