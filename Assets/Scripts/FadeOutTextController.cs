using System.Collections;
using UnityEngine;
using TMPro;

public class FadeOutTextController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI textUI;
    public float displayTime = 2f;       // 텍스트 보여지는 시간
    public float fadeDuration = 2f;      // 페이드아웃 지속 시간

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    public void ShowText(string message)
    {
        textUI.text = message;
        gameObject.SetActive(true);
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(displayTime);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        gameObject.SetActive(false);
    }
}