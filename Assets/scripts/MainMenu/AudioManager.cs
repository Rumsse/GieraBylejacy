using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("---Audio Source---")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource soundsSource;

    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    [Header("---Audio Clips inGame---")]
    public AudioClip walkingMC;
    public AudioClip flyingBAT;

    [Header("---Buttons Sounds---")]
    public AudioClip buttonClicked;
    public AudioClip buttonHover;



    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    public void PlaySounds()
    {
        
    }

    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        mixer.SetFloat("master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mixer.SetFloat("music", Mathf.Log10(volume)*20);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        mixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
    }

    public void Buttons(AudioClip clip)
    {
        soundsSource.PlayOneShot(clip);
    }

}
