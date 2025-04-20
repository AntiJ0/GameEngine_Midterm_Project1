using UnityEngine;
using System.Collections;

public class FallingObjectSpawner : MonoBehaviour
{
    public GameObject fallingObjectPrefab;
    public Transform[] spawnPoints;
    public float interval = 2f;
    public float fallSpeed = 5f;
    public float lifetime = 3f;

    public void StartPattern(float duration)
    {
        Debug.Log("✅ StartPattern 호출됨");
        StartCoroutine(SpawnRoutine(duration));
    }

    private IEnumerator SpawnRoutine(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject obj = Instantiate(fallingObjectPrefab, point.position, Quaternion.identity);

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.velocity = Vector2.down * fallSpeed;

            Destroy(obj, lifetime);

            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        Debug.Log("🛑 낙하물 패턴 종료");
    }
}
