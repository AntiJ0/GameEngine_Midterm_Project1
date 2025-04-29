using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public GameObject popupPanel; // Inspector�� �˾�â�� �巡���س��� ����

    public void ShowPopup()
    {
        popupPanel.SetActive(true); // ��ư�� ������ �˾��� Ȱ��ȭ
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}
