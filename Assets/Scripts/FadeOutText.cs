using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeOutText : MonoBehaviour
{
    public TMP_Text messageText;
    public float displayTime = 2.5f;      // 보여지는 시간
    public float fadeDuration = 1f;       // 페이드아웃 시간

    void Start()
    {
        StartCoroutine(ShowAndFadeText());
    }

    IEnumerator ShowAndFadeText()
    {
        // 텍스트를 완전히 보이게 설정
        Color startColor = messageText.color;
        startColor.a = 1f;
        messageText.color = startColor;
        messageText.gameObject.SetActive(true);

        // 일정 시간 대기
        yield return new WaitForSeconds(displayTime);

        // 페이드아웃
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            Color c = messageText.color;
            c.a = alpha;
            messageText.color = c;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 마지막 알파값 0으로 고정하고 비활성화
        Color finalColor = messageText.color;
        finalColor.a = 0f;
        messageText.color = finalColor;
        messageText.gameObject.SetActive(false);
    }
}
