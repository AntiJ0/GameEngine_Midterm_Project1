using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform[] firePoints;
    public float shootInterval = 1.5f;

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
            Transform firePoint = firePoints[Random.Range(0, firePoints.Length)];

            if (projectilePrefab != null)
            {
                GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0f, 0f, 90f));
                Debug.Log("🧨 투사체 생성 at " + firePoint.position);
            }

            yield return new WaitForSeconds(shootInterval);
            elapsed += shootInterval;
        }

        Debug.Log("🛑 투사체 패턴 종료");
    }
}