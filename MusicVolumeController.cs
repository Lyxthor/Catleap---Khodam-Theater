using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeController : MonoBehaviour
{
    private Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(SetVolume);
    }
    private void OnEnable()
    {
        slider.value = AudioManager.Instance.MusicVolume;
        SetVolume(slider.value);
    }
    private void SetVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }
}
