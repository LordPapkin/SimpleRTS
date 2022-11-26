using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{    
    [Header("Buttons")]
    [SerializeField] private Button soundIncreaseButton;
    [SerializeField] private Button soundDecreaseButton;
    [SerializeField] private Button musicIncreaseButton;
    [SerializeField] private Button musicDecreaseButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Toggle edgeScrollingToggle;
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI soundVolumeText;
    [SerializeField] private TextMeshProUGUI musicVolumeText;   
    
    private void Awake()
    {
        soundIncreaseButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.IncreaseVolume();
            UpdateSoundVolumeText();
        });
        soundDecreaseButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.DecreaseVolume();
            UpdateSoundVolumeText();
        });
        musicIncreaseButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.IncreaseVolume();
            UpdateMusicVolumeText();
        });
        musicDecreaseButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.DecreaseVolume();
            UpdateMusicVolumeText();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });        
        edgeScrollingToggle.onValueChanged.AddListener((bool set) =>
        {
            CameraHandler.Instance.ToggleEdgeScrolling(set);
        });
       
    }

    private void Start()
    {
        UpdateSoundVolumeText();
        UpdateMusicVolumeText();
        gameObject.SetActive(false);
        edgeScrollingToggle.SetIsOnWithoutNotify(CameraHandler.Instance.IsEdgeScrollingEnabled);
    }

    public void ToggleVisible()
    {       
        gameObject.SetActive(!gameObject.activeSelf);
        if (gameObject.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
  
    private void UpdateSoundVolumeText()
    {
        soundVolumeText.SetText(Mathf.RoundToInt(SoundManager.Instance.Volume * 10).ToString());
    }

    private void UpdateMusicVolumeText()
    {
        musicVolumeText.SetText(Mathf.RoundToInt(MusicManager.Instance.Volume * 10).ToString());
    }
}
