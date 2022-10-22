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

    [SerializeField] private AudioSource audioSource;
    private Dictionary<Sound, AudioClip> audioClipDictionary;

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

    public void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(audioClipDictionary[sound]);
    }
}
