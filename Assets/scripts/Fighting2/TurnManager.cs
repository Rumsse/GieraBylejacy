using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class TurnManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public EnemyMovement enemyMovement;
    public BatMateMovement batMateMovement;
    public PlayerAbilities playerAbilities;
    public EnemyAbilities enemyAbilities;
    public BatMateAbilities batMateAbilities;

    public Button BasicAttackButton;
    public Button AdvancedAttackButton;

    public bool isPlayerTurn = true;
    public bool isBatMateTurn = true;
    public bool canUseAdvancedAttack = true;
    public float enemyMoveDelay = 1.3f;
    public int turnCounter = 0;

    public HexTilemapPathfinding hexTilemapPathfinding;
    public Vector3Int startPosition;
    public Vector3Int targetPosition;

    public TurnState currentTurn = TurnState.Player;


    public Image turnImage;
    public Sprite[] characterSprites;
    public GameObject[] characters;
    private int currentTurnIndex = 0;


    public enum TurnState
    {
        Player,
        BatMate,
        Enemy
    }

    private void Start()
    {
        currentTurn = TurnState.Player;
        UpdateTurnIndicator();
        StartTurn();
    }


    void StartTurn()
    {
        switch (currentTurn)
        {
            case TurnState.Player:
                playerMovement.isActive = true;
                batMateMovement.isActive = false;
                enemyMovement.isActive = false;
                StartPlayerTurn();
                break;
            case TurnState.BatMate:
                playerMovement.isActive = false;
                batMateMovement.isActive = true;
                enemyMovement.isActive = false;
                StartBatMateTurn();
                break;
            case TurnState.Enemy:
                playerMovement.isActive = false;
                batMateMovement.isActive = false;
                enemyMovement.isActive = true;
                StartEnemyTurn();
                break;
        }
    }


    void StartPlayerTurn()
    {
        playerMovement.ResetMovement();
        Debug.Log("Tura gracza");

        turnCounter++;
        canUseAdvancedAttack = (turnCounter % 2 == 1);

        if (playerMovement.hasMoved && Input.GetKeyDown(KeyCode.Space))
        {
            EndPlayerTurn();
        }
    }

    void EndPlayerTurn()
    {
        Debug.Log("Koniec tury gracza");

        playerAbilities.isAttackMode1 = false;
        playerAbilities.isAttackMode2 = false;
        enemyAbilities.hasTakenDamage = false;

        BasicAttackButton.GetComponent<Image>().color = Color.white;
        AdvancedAttackButton.GetComponent<Image>().color = Color.white;

        currentTurn = (TurnState)(((int)currentTurn + 1) % 3);
        currentTurnIndex = (currentTurnIndex + 1) % characters.Length;
        UpdateTurnIndicator();

        StartTurn();
    }

    void StartBatMateTurn()
    {
        batMateMovement.ResetMovement();
        Debug.Log("Tura Bat Mate'a");

        if (batMateMovement.hasMoved && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Koniec3");
            EndBatMateTurn();
            Debug.Log("Koniec4");
        }
    }

    void EndBatMateTurn()
    {
        Debug.Log("Koniec tury bat mate'a");
        batMateAbilities.isAttackMode1 = false;
        enemyAbilities.hasTakenDamage = false;

        BasicAttackButton.GetComponent<Image>().color = Color.white;

        currentTurn = (TurnState)(((int)currentTurn + 1) % 3);
        currentTurnIndex = (currentTurnIndex + 1) % characters.Length;
        UpdateTurnIndicator();

        StartTurn();
        Debug.Log("Koniec2");

    }

    System.Collections.IEnumerator StartEnemyTurnWithDelay()
    {
        yield return new WaitForSeconds(enemyMoveDelay);
        Debug.Log("OpóŸnienie zakoñczone, zaczynam turê przeciwnika.");
        StartEnemyTurn();
    }

    void StartEnemyTurn()
    {
        Debug.Log("Tura przeciwnika");

        Vector3Int playerHexPos = playerMovement.hexTilemap.WorldToCell(playerMovement.transform.position); //Pobiera pozycje gracza i przeciwnika
        Vector3Int enemyHexPos = enemyMovement.hexTilemap.WorldToCell(enemyMovement.transform.position);

        enemyMovement.isEnemyMoving = true;


        if (enemyMovement.hasMoved)
        {
            EndEnemyTurn();
        }
    }

    public void EndEnemyTurn()
    {
        Debug.Log("Koniec tury przeciwnika");
        currentTurn = (TurnState)(((int)currentTurn + 1) % 3);
        currentTurnIndex = (currentTurnIndex + 1) % characters.Length;
        UpdateTurnIndicator();

        StartTurn();
    }


    public void UpdateTurnIndicator()
    {
        turnImage.sprite = characterSprites[currentTurnIndex];
    }







    bool IsInAttackRange(Vector3 attackerPos, Vector3 targetPos) // OGARN¥Æ TO W JAKIMŒ INNYM SKRYPCIE CZY COŒ
    {
        // Pobierz pozycje heksów na siatce Tilemap
        Vector3Int attackerHexPos = playerMovement.hexTilemap.WorldToCell(attackerPos);
        Vector3Int targetHexPos = playerMovement.hexTilemap.WorldToCell(targetPos);

        // Zamieñ pozycje na obiekty Hex
        Hex attackerHex = new Hex(attackerHexPos.x, attackerHexPos.y);
        Hex targetHex = new Hex(targetHexPos.x, targetHexPos.y);

        // SprawdŸ, czy target znajduje siê w s¹siaduj¹cych heksach
        foreach (var neighbor in attackerHex.GetAllNeighbors())
        {
            if (neighbor.q == targetHex.q && neighbor.r == targetHex.r)
            {
                return true; // Target jest w zasiêgu ataku
            }
        }

        return false; // Target nie jest w zasiêgu ataku
    }


}




