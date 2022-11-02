using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager Instance => instance;
    private static HighscoreManager instance;

    private int score;
    private int highScore;

    public void AddScore(int value)
    {
        score += value;
    }

    public void SubtractionScore(int value)
    {        
        score -= value;        
    }

    public void GameEnded()
    {
        bool isNewHighScore = score > highScore;
        if (isNewHighScore)
            PlayerPrefs.SetInt("highScore", score);

        GameOverUI.Instance.ShowHighScore(isNewHighScore, score, highScore);        
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        highScore = PlayerPrefs.GetInt("highScore", 0);  
        Debug.Log(highScore);
    }
}
