using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public Text scoreText;
    public Text highScoreText;

    float score;
    float highScore;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        score = PlayerPrefs.GetFloat("SCORE", 0);
        highScore = PlayerPrefs.GetFloat("HIGHSCORE", 0);
        scoreText.text = score.ToString() + " POINTS";
        highScoreText.text = "HIGHSCORE: " + highScore.ToString();
    }

    public void addPoint(float point)
    {
        score += point;
        scoreText.text = score.ToString() + " POINTS";

        PlayerPrefs.SetFloat("SCORE", score);
    }

    public void subtractPoint(float point)
    {
        score -= point;
        scoreText.text = score.ToString() + " POINTS";

        PlayerPrefs.SetFloat("SCORE", score);
    }

    public float getScore()
    {
        return score;
    }

    public float getHighScore()
    {
        return highScore;
    }
}
