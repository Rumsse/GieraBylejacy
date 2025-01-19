using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject WinScreen;

    public void WinScreenPause()
    {
        WinScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void NextLevel()
    {
        WinScreen.SetActive(false);
        Time.timeScale = 1;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(currentSceneIndex + 1);
    }

}
