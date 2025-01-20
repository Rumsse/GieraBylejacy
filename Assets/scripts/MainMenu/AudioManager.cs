using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource soundsSource;

    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;

    public AudioClip walkingMC;
    public AudioClip flyingBAT;

    private void Start()
    {
        
    }

    public void PlaySounds()
    {
        
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("music", Mathf.Log10(volume)*20);
    }

}
