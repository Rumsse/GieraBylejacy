using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CharactersData[] characters;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // GameManager przetrwa zmianê sceny
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetGame()
    {
        foreach (var character in characters)
        {
            character.ResetData();
        }

    }

    public void OnPlayerDeath()
    {
        SceneManager.LoadScene("GameOver");
    }

}
