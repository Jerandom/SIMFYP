using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighscoreResult : MonoBehaviour
{
    private void OnEnable()
    {
        TextMeshProUGUI text = this.GetComponent<TextMeshProUGUI>();

        float score = PlayerPrefs.GetFloat("SCORE");

        string finalText = "By the way, your total score is: " + score + "\nCan you do better next time ? ";

        text.text = finalText;

        if (ScoreManager.instance.getHighScore() < ScoreManager.instance.getScore())
            PlayerPrefs.SetFloat("HIGHSCORE", score); ;
    }
}
