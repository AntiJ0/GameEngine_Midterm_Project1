using UnityEngine;
using System.Collections;

public class SpikeSpawner : MonoBehaviour
{
    public GameObject spikePrefab;
    public GameObject warningPrefab;
    public Transform[] spawnPoints;
    public float spawnInterval = 1f;
    public float warningDuration = 0.5f;
    public float spikeLifetime = 2f;

    private Coroutine routine;

    public void StartPattern(float duration)
    {
        if (routine != null)
            StopCoroutine(routine);
        routine = StartCoroutine(SpawnSpikeRoutine(duration));
    }

    private IEnumerator SpawnSpikeRoutine(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject warning = Instantiate(warningPrefab, point.position, Quaternion.identity);
            yield return new WaitForSeconds(warningDuration);
            Destroy(warning);

            GameObject spike = Instantiate(spikePrefab, point.position, Quaternion.identity);
            Destroy(spike, spikeLifetime);

            yield return new WaitForSeconds(spawnInterval);
            elapsed += spawnInterval;
        }
    }
}