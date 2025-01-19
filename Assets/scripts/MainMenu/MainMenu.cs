using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator transitionAnimator;
    public AudioManager audioManager;

    public float transitionTime = 1f;
    public GameObject optionsPanel;


    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoBackToMainMenu()
    {
        audioManager.Buttons(audioManager.buttonClicked);
        StartCoroutine(LoadSceneWithTransition(0));
    }

    public IEnumerator LoadSceneWithTransition(int sceneIndex)
    {
        transitionAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadSceneAsync(0);
    }

    public void ExitOptionPanel()
    {
        audioManager.Buttons(audioManager.buttonClicked);
        optionsPanel.SetActive(false);
    }


}
