using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance => Instace;
    private static GameOverUI Instace;

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private TextMeshProUGUI survivedWavesText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private TextMeshProUGUI newHighscoreText;

    private void Awake()
    {
        Hide();
        if (Instace == null)
            Instace = this;

        mainMenuButton.onClick.AddListener(() => 
        {
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });
        retryButton.onClick.AddListener(() => 
        { 
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
        });
    }

    public void Show()
    {
        int currentWave = EnemyWaveManager.Instance.WaveNumber;
        gameObject.SetActive(true);

        if(currentWave == 2)
            survivedWavesText.SetText($"You Survived {currentWave - 1} Wave");
        else
            survivedWavesText.SetText($"You Survived {currentWave - 1} Waves");

        HighscoreManager.Instance.GameEnded();
    }

    public void ShowHighScore(bool isNewHighScore, int score, int highScore)
    {
        if (isNewHighScore)
        {
            newHighscoreText.SetText("New highscore: " + score.ToString());
            newHighscoreText.gameObject.SetActive(true);
        }
        else
        {
            scoreText.SetText("Your score: " + score.ToString());
            highscoreText.SetText("Your highscore: " + highScore.ToString());
            scoreText.gameObject.SetActive(true);
            highscoreText.gameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
