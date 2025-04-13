using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TMPHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI targetText;     // 색 바꿀 텍스트
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;
    public Color pressedColor = Color.red;

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetText.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetText.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetText.color = hoverColor;
    }
}
