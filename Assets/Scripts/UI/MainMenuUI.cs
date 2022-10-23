using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button ExitButton;

    private void Awake()
    {
        PlayButton.onClick.AddListener(() =>
        {
            GameSceneManager.Load(GameSceneManager.Scene.GameScene);
        });
        ExitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
