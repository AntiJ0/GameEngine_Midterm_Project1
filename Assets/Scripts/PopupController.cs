using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public GameObject popupPanel; // Inspector에 팝업창을 드래그해놓을 변수

    public void ShowPopup()
    {
        popupPanel.SetActive(true); // 버튼을 누르면 팝업을 활성화
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}
