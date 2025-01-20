using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using UnityEngine.UI;


public class TurnManagerPuzzle : MonoBehaviour
{
    public PlayerMovementPuzzle playerMovement;
    public BatMateMovementPuzzle batMateMovement;
    public TileManagerPuzzle tileManager;
    public Tilemap hexTilemap;

    public TurnState currentTurn = TurnState.Player;
    public int turnCounter = 0;
    private int currentTurnIndex = 0;

    public Image turnImage;
    public Sprite[] characterSprites;
    public GameObject[] characters;
    public List<GameObject> activeCharacters = new List<GameObject>();


    public enum TurnState
    {
        Player,
        BatMate,
    }

    private void Start()
    {
        activeCharacters.Add(playerMovement.gameObject);
        activeCharacters.Add(batMateMovement.gameObject);

        currentTurn = TurnState.Player;
        UpdateTurnIndicator();
        StartTurn();
    }


    void StartTurn()
    {
        if (activeCharacters.Count == 0)
        {
            return;
        }

        GameObject currentCharacter = activeCharacters[currentTurnIndex];

        if (currentCharacter == null)
        {
            NextTurn();
            return;
        }

        while (!activeCharacters[currentTurnIndex].activeSelf)
        {
            currentTurnIndex = (currentTurnIndex + 1) % activeCharacters.Count;
            currentTurn = (TurnState)(((int)currentTurn + 1) % 2);
        }

        switch (currentTurn)
        {
            case TurnState.Player:
                playerMovement.isActive = true;
                batMateMovement.isActive = false;
                StartPlayerTurn();
                break;
            case TurnState.BatMate:
                playerMovement.isActive = false;
                batMateMovement.isActive = true;
                StartBatMateTurn();
                break;
        }
    }

    void NextTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % activeCharacters.Count;
        currentTurn = (TurnState)(((int)currentTurn + 1) % 2);
        UpdateTurnIndicator();
        StartTurn();
    }


    void StartPlayerTurn()
    {
        playerMovement.ResetMovement();
        Debug.Log("Tura gracza");

        if (playerMovement.hasMoved && Input.GetKeyDown(KeyCode.Space))
        {
            EndPlayerTurn();
        }
    }

    public void EndPlayerTurn()
    {
        Debug.Log("Koniec tury gracza");
        NextTurn();
    }

    void StartBatMateTurn()
    {
        batMateMovement.ResetMovement();
        Debug.Log("Tura Bat Mate'a");

        if (batMateMovement.hasMoved && Input.GetKeyDown(KeyCode.Space))
        {
            EndBatMateTurn();
        }
    }

    void EndBatMateTurn()
    {
        Debug.Log("Koniec tury bat mate'a");
        NextTurn();
    }

    public void UpdateTurnIndicator()
    {

        if (activeCharacters.Count == 0)
        {
            Debug.Log("Brak aktywnych postaci! Gra zakoñczona.");
            return;
        }

        GameObject currentCharacter = activeCharacters[currentTurnIndex];

        switch (currentTurn)
        {
            case TurnState.Player:
                turnImage.sprite = characterSprites[0];
                break;
            case TurnState.BatMate:
                turnImage.sprite = characterSprites[1];
                break;
        }
    }

    public void OnCharacterDeath(GameObject deadCharacter)
    {
        if (activeCharacters.Contains(deadCharacter))
        {
            activeCharacters.Remove(deadCharacter);
            Debug.Log(deadCharacter.name + " zosta³ usuniêty z aktywnych postaci.");

            Vector3Int currentHexPosition = hexTilemap.WorldToCell(transform.position);
            tileManager.ReleaseTile(currentHexPosition);

            if (activeCharacters.Count > 0 && activeCharacters[currentTurnIndex] == deadCharacter)
            {
                currentTurnIndex %= activeCharacters.Count;
            }
        }
    }


}




