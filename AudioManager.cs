using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Sound
{
    public string audioName;
    public AudioClip audioClip;
}
public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find an existing instance of the class in the scene
                _instance = FindObjectOfType<AudioManager>();

                if (_instance == null)
                {
                    // Create a new GameObject and add the SingletonExample component
                    GameObject singletonObject = new GameObject();
                    _instance = singletonObject.AddComponent<AudioManager>();
                    singletonObject.name = typeof(AudioManager).ToString() + " (Singleton)";
                }
            }
            return _instance;
        }
    }
    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private string musicSourceName;
    private float musicVolume=0.5f;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public string MusicSourceName { get => musicSourceName; }
    public float MusicVolume { get => musicVolume; }
    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(musicSounds, (x) => x.audioName == name);
        if (sound == null) return;
        musicSourceName = name;
        musicSource.clip = sound.audioClip;
        musicSource.Play();
    }
    public void PlaySfx(string name)
    {
        Sound sound = Array.Find(musicSounds, (x) => x.audioName == name);
        if (sound == null) return;
        sfxSource.PlayOneShot(sound.audioClip);
    }
    public void StopMusic()
    {
        if (musicSource.isPlaying) musicSource.Stop();
    }
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = Mathf.Clamp(volume, 0.0f, 1.0f);
    }
}
