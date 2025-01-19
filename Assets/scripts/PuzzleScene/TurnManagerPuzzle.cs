using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public class TurnManagerPuzzle : MonoBehaviour
{
    public PlayerMovementPuzzle playerMovement;
    public BatMateMovementPuzzle batMateMovement;

    public AudioManager audioManager;

    public bool isPlayerTurn = true;

    public Vector3Int startPosition;
    public Vector3Int targetPosition;

    public Image turnImage;
    public Sprite[] characterSprites;
    public GameObject[] characters;
    private int currentTurnIndex = 0;

    public int turnCounter = 0;


    void Start()
    {
        UpdateTurnIndicator();
    }

    void Update()
    {
        if (isPlayerTurn)
        {
            if (playerMovement.hasMoved && Input.GetKeyDown(KeyCode.Space))
            {
                EndPlayerTurn();
            }
        }
        else
        {
            if (batMateMovement.hasMoved && Input.GetKeyDown(KeyCode.Space))
            {
                EndBatMateTurn();
            }
        }
    }

    void StartPlayerTurn()
    {
        isPlayerTurn = true;
        playerMovement.ResetMovement();
        Debug.Log("Tura gracza");
    }

    public void EndPlayerTurn()
    {
        audioManager.Buttons(audioManager.buttonClicked);

        isPlayerTurn = false;
        Debug.Log("Koniec tury gracza");


        currentTurnIndex = (currentTurnIndex + 1) % characters.Length;
        UpdateTurnIndicator();

        StartBatMateTurn();
    }

    void StartBatMateTurn()
    {
        isPlayerTurn = false;
        Debug.Log("Tura przeciwnika");

        //.isBatMateMoving = true;
    }

    public void EndBatMateTurn()
    {
        isPlayerTurn = true;
        batMateMovement.isBatMateMoving = false;
        Debug.Log("Koniec tury batmata");

        currentTurnIndex = (currentTurnIndex + 1) % characters.Length;
        UpdateTurnIndicator();

        StartPlayerTurn();
    }

    public void UpdateTurnIndicator()
    {
        turnImage.sprite = characterSprites[currentTurnIndex];
    }







}
