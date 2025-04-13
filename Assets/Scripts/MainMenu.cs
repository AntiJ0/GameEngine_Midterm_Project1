using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnClickStartGame()
    {
        string lastStage = PlayerPrefs.GetString("LastPlayedStage", "Stage1"); // 기본값은 Stage1

        SceneManager.LoadScene(lastStage);
    }

    public void OnClickNewGame()
    {
        PlayerPrefs.DeleteKey("LastPlayedStage"); // 기존 기록 제거
        SceneManager.LoadScene("Stage1");         // Stage1부터 시작
    }
}
