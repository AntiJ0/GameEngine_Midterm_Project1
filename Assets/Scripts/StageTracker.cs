using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageTracker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string currentStage = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("LastPlayedStage", currentStage);
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
