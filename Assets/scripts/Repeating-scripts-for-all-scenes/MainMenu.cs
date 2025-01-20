using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public Animator transitionAnimator;
    public AudioManager audioManager;
    public GameManager gameManager;

    public float transitionTime = 1f;
    public GameObject optionsPanel;


    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        audioManager.Buttons(audioManager.buttonClicked);
        Application.Quit();
    }

    public void GoBackToMainMenu()
    {
        audioManager.Buttons(audioManager.buttonClicked);
        gameManager.ResetGame();
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
