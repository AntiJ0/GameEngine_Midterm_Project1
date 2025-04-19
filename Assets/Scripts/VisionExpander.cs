using UnityEngine;

public class VisionExpander : MonoBehaviour
{
    public Transform visionMask;
    public float expandSpeed = 1.5f;
    public float maxScale = 100f;  // 넉넉하게 설정

    private bool expanding = false;

    void Start()
    {
        if (visionMask != null)
            visionMask.localScale = new Vector3(56.62498f, 56.62498f, 1f);
    }

    void Update()
    {
        if (visionMask != null)
        {
            if (expanding)
            {
                float current = visionMask.localScale.x;
                float newScale = Mathf.Min(current + expandSpeed * Time.deltaTime, maxScale);
                visionMask.localScale = new Vector3(newScale, newScale, 1f);
            }
        }
    }

    public void StartExpandingVision()
    {
        if (expanding) return; // 이미 확장 중이면 무시
        expanding = true;
    }
}