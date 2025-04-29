using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    public TextMeshProUGUI[] scoreTexts;

    private void OnEnable() 
    {
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            int stage = i + 1;
            int score = HighScore.Load(stage); 
            scoreTexts[i].text = "Stage " + stage + " : " + score.ToString();
        }
    }
}