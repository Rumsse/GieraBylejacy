using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public CharacterData[] characters;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // GameManager przetrwa zmian� sceny
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
