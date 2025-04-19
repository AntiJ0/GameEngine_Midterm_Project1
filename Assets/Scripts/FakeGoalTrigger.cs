using UnityEngine;

public class FakeGoalTrigger : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;

            VisionExpander visionExpander = FindObjectOfType<VisionExpander>();
            if (visionExpander != null)
            {
                visionExpander.StartExpandingVision();
            }

            // 원한다면: 사운드, 보스 등장 등도 이 안에 넣으면 돼
        }
    }
}