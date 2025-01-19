using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
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



    public AudioClip[] musicClips;
    public float[] pitchValues;




    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        musicSource.Stop();

        PlayMusic(scene.buildIndex);
    }

    public void PlayMusic(int sceneIndex)
    {

        if (musicSource != null)
        {
            musicSource.Stop(); 
            musicSource.clip = musicClips[sceneIndex];
            musicSource.pitch = GetPitchForScene(sceneIndex);
            musicSource.Play(); 
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private float GetPitchForScene(int sceneIndex)
    {
        if (sceneIndex < pitchValues.Length)
        {
            return pitchValues[sceneIndex];
        }
        else
        {
            return 1f; 
        }
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
