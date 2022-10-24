using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance => instance;
    private static MusicManager instance;

    public float Volume => volume;

    [SerializeField] private AudioSource musicAudioSource;
    private float volume = 0.5f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        volume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        musicAudioSource.volume = volume;
    }

    public void IncreaseVolume()
    {
        volume += 0.1f;
        Mathf.Clamp01(volume);
        musicAudioSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void DecreaseVolume()
    {
        volume -= 0.1f;
        Mathf.Clamp01(volume);
        musicAudioSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
   
}
