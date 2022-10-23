using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum Sound 
    { 
        BuildingPlaced,
        BuildingDamaged,
        BuildingDestroyed,
        EnemyDie,
        EnemyHit,
        GameOver
    }

    public static SoundManager Instance => instance;
    public static SoundManager instance;

    public float Volume => volume;

    [SerializeField] private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> audioClipDictionary;
    private float volume = 0.5f;
    public void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(audioClipDictionary[sound], volume);
    }

    public void IncreaseVolume()
    {
        volume += 0.1f;
        volume = Mathf.Clamp01(volume);
    }

    public void DecreaseVolume()
    {
        volume -= 0.1f;
        volume = Mathf.Clamp01(volume);
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        LoadSounds();
    }

    private void LoadSounds()
    {
        audioClipDictionary = new Dictionary<Sound, AudioClip>();
        foreach (Sound sound in System.Enum.GetValues(typeof(Sound)))
        {
            audioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
    }
   
}
