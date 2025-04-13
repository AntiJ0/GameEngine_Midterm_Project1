using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    public static string previousSceneName;
    public string nextLevel;

    void Start()
    {
        // 현재 씬을 시작할 때, 이전 씬으로 저장해 둠
        previousSceneName = SceneManager.GetActiveScene().name;
    }

    public void LoadPreviousScene()
    {
        if (!string.IsNullOrEmpty(previousSceneName))
        {
            SceneManager.LoadScene(previousSceneName);
        }
        else
        {
            Debug.LogWarning("이전 씬 정보가 없습니다!");
        }
    }

    public void MoveToNextLevel()
    {
        SceneManager.LoadScene(nextLevel);
    }

    public void LoadStage1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stage1");
    }

    public void LoadMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
