using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class PatternController : MonoBehaviour
{
    public SpikeSpawner spikeSpawner;
    public FallingObjectSpawner fallingSpawner;
    public ProjectileSpawner projectileSpawner;

    public GameObject bossRoomTilemap;

    private Camera mainCamera;
    private Camera bossCamera;

    public float patternDuration = 10f;

    public void StartPatternSequence(Camera mainCam, Camera bossCam)
    {
        mainCamera = mainCam;
        bossCamera = bossCam;

        StartCoroutine(PatternRoutine());
    }

    private IEnumerator PatternRoutine()
    {
        // 🌵 가시 패턴
        Debug.Log("🌵 가시 패턴 시작");
        spikeSpawner.StartPattern(patternDuration);
        yield return new WaitForSeconds(patternDuration);

        // 🌩 낙석 패턴
        Debug.Log("🌩 낙석 패턴 시작");
        fallingSpawner.StartPattern(patternDuration);
        yield return new WaitForSeconds(patternDuration);

        // 🔫 투사체 패턴
        Debug.Log("🔫 투사체 패턴 시작");
        projectileSpawner.StartPattern(patternDuration);
        yield return new WaitForSeconds(15f); // 마지막 패턴 완료 대기

        // 🧱 타일맵 비활성화
        if (bossRoomTilemap != null)
        {
            var renderer = bossRoomTilemap.GetComponent<TilemapRenderer>();
            var collider = bossRoomTilemap.GetComponent<TilemapCollider2D>();
            if (renderer != null) renderer.enabled = false;
            if (collider != null) collider.enabled = false;
        }

        // 🎥 카메라 복구 (BossCamera → MainCamera)
        if (mainCamera != null)
        {
            mainCamera.enabled = true;
            Debug.Log("MainCamera 다시 켜짐");
        }
        else
        {
            Debug.LogError("MainCamera 연결 안됨");
        }

        if (bossCamera != null)
        {
            bossCamera.enabled = false;
            Debug.Log("BossCamera 다시 꺼짐");
        }
        else
        {
            Debug.LogError("BossCamera 연결 안됨");
        }

        Debug.Log("보스 패턴 종료됨. 카메라 복구 완료.");
    }
}