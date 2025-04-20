using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class FakeGoalTrigger : MonoBehaviour
{
    public GameObject bossRoomTilemapObject;
    public PatternController patternController;

    public GameObject playerObject;
    public Camera mainCamera;
    public Camera bossCamera;

    public FadeOutTextController fadeOutText;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            Debug.Log("FakeGoal 트리거 발동됨!");

            if (fadeOutText != null)
            {
                fadeOutText.ShowText("살아남으세요!");
            }

            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;

            FindObjectOfType<VisionExpander>()?.StartExpandingVision();

            if (bossRoomTilemapObject != null)
            {
                var renderer = bossRoomTilemapObject.GetComponent<TilemapRenderer>();
                var collider = bossRoomTilemapObject.GetComponent<TilemapCollider2D>();

                if (renderer != null) renderer.enabled = true;
                if (collider != null) collider.enabled = true;
            }

            if (mainCamera != null)
            {
                mainCamera.enabled = false;
                Debug.Log("MainCamera 끔");
            }
            else
            {
                Debug.LogError("MainCamera 연결 안됨");
            }

            if (bossCamera != null)
            {
                bossCamera.enabled = true;
                Debug.Log("BossCamera 켜짐");
            }
            else
            {
                Debug.LogError("BossCamera 연결 안됨");
            }

            // 이동 제한
            if (playerObject != null)
            {
                var controller = playerObject.GetComponent<PlayerController>();
                if (controller != null)
                {
                    controller.canMove = false;
                }
            }

            // ✅ 이거 빠져 있어서 아무 일도 안 났던 거야!
            StartCoroutine(EnablePlayerMovementAfterDelay());
        }
    }

    private IEnumerator EnablePlayerMovementAfterDelay()
    {
        yield return new WaitForSeconds(7f);

        if (playerObject != null)
        {
            var controller = playerObject.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.canMove = true;
            }
        }

        if (patternController != null)
        {
            patternController.StartPatternSequence(mainCamera, bossCamera);
        }
    }
}