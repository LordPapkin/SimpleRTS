using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance => instace;
    public static GameOverUI instace;

    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private TextMeshProUGUI survivedWavesText;

    private void Awake()
    {
        Hide();
        if (instace == null)
            instace = this;

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
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
