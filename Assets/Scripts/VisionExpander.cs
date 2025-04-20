using UnityEngine;
using System.Collections; // 🔧 이게 없으면 IEnumerator 못 씀!

public class VisionExpander : MonoBehaviour
{
    public Transform maskTransform; // SpriteMask 오브젝트의 Transform
    public Vector3 targetScale = new Vector3(10f, 10f, 1f); // 목표 크기
    public float expandDuration = 2f; // 확장 시간

    public void StartExpandingVision()
    {
        StartCoroutine(ExpandMask());
    }

    private IEnumerator ExpandMask()
    {
        Vector3 initialScale = maskTransform.localScale;
        float elapsed = 0f;

        while (elapsed < expandDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / expandDuration;
            maskTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }

        maskTransform.localScale = targetScale;
    }
}