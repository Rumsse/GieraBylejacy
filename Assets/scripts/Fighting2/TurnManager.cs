using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;


public class TurnManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public EnemyMovement enemyMovement;
    public Enemy2Movement enemy2Movement;
    public BatMateMovement batMateMovement;
    public PlayerAbilities playerAbilities;
    public EnemyAbilities enemyAbilities;
    public Enemy2Abilities enemy2Abilities;
    public BatMateAbilities batMateAbilities;

    public Button BasicAttackButton;
    public Button AdvancedAttackButton;

    public bool isEnemyTurn;
    public float enemyMoveDelay = 1.3f;

    public TurnState currentTurn = TurnState.Player;
    public int turnCounter = 0;
    private int currentTurnIndex = 0;

    public Image turnImage;
    public Sprite[] characterSprites;
    public GameObject[] characters;
    public List<GameObject> activeCharacters;


    public enum TurnState
    {
        Player,
        BatMate,
        Enemy1,
        Enemy2
    }

    private void Start()
    {
        activeCharacters.Add(playerMovement.gameObject);
        activeCharacters.Add(batMateMovement.gameObject);
        activeCharacters.Add(enemyMovement.gameObject);
        activeCharacters.Add(enemy2Movement.gameObject);

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

        if (!activeCharacters[currentTurnIndex].activeSelf)
        {
            currentTurnIndex = (currentTurnIndex + 1) % activeCharacters.Count;
        }

        GameObject currentCharacter = activeCharacters[currentTurnIndex];

        switch (currentTurn)
        {
            case TurnState.Player:
                playerMovement.isActive = true;
                batMateMovement.isActive = false;
                enemyMovement.isActive = false;
                enemy2Movement.isActive = false;
                StartPlayerTurn();
                break;
            case TurnState.BatMate:
                if (batMateAbilities.isAlive)
                {
                    playerMovement.isActive = false;
                    batMateMovement.isActive = true;
                    enemyMovement.isActive = false;
                    enemy2Movement.isActive = false;
                    StartBatMateTurn();
                }
                else
                {
                    EndBatMateTurn();
                }
                break;
            case TurnState.Enemy1:
                if (enemyAbilities.isAlive)
                {
                    playerMovement.isActive = false;
                    batMateMovement.isActive = false;
                    enemyMovement.isActive = true;
                    enemy2Movement.isActive = false;
                    StartEnemyTurn();
                }
                else
                {
                    EndEnemyTurn();
                }
                break;
            case TurnState.Enemy2:
                if (enemy2Abilities.isAlive)
                {
                    playerMovement.isActive = false;
                    batMateMovement.isActive = false;
                    enemyMovement.isActive = false;
                    enemy2Movement.isActive = true;
                    StartEnemy2Turn(); 
                }
                else
                {
                    EndEnemy2Turn();
                }
                break;
        }
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

    void EndPlayerTurn()
    {
        Debug.Log("Koniec tury gracza");

        playerAbilities.isAttackMode1 = false;
        playerAbilities.isAttackMode2 = false;
        enemyAbilities.hasTakenDamage = false;
        enemy2Abilities.hasTakenDamage = false;

        BasicAttackButton.GetComponent<Image>().color = Color.white;
        AdvancedAttackButton.GetComponent<Image>().color = Color.white;

        currentTurn = (TurnState)(((int)currentTurn + 1) % 4);
        currentTurnIndex = (currentTurnIndex + 1) % characters.Length;
        UpdateTurnIndicator();

        playerAbilities.OnOneTurnEnd();

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
        enemy2Abilities.hasTakenDamage = false;

        BasicAttackButton.GetComponent<Image>().color = Color.white;

        currentTurn = (TurnState)(((int)currentTurn + 1) % 4);
        currentTurnIndex = (currentTurnIndex + 1) % characters.Length;
        UpdateTurnIndicator();

        StartTurn();
        Debug.Log("Koniec2");

    }

    System.Collections.IEnumerator StartEnemyTurnWithDelay() //useless for now
    {
        yield return new WaitForSeconds(enemyMoveDelay);
        Debug.Log("Op�nienie zako�czone, zaczynam tur� przeciwnika.");
        StartEnemyTurn();
    }

    void StartEnemyTurn()
    {
        isEnemyTurn = true;
        Debug.Log("Tura przeciwnika");
        enemyMovement.ResetMovement();

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
        currentTurn = (TurnState)(((int)currentTurn + 1) % 4);
        currentTurnIndex = (currentTurnIndex + 1) % characters.Length;
        UpdateTurnIndicator();
        isEnemyTurn = false;

        StartTurn();
    }

    public void StartEnemy2Turn()
    {
        isEnemyTurn = true;
        Debug.Log("Tura drugiego przeciwnika");
        enemy2Movement.ResetMovement();

        Vector3Int playerHexPos = playerMovement.hexTilemap.WorldToCell(playerMovement.transform.position); //Pobiera pozycje gracza i przeciwnika
        Vector3Int enemyHexPos = enemyMovement.hexTilemap.WorldToCell(enemyMovement.transform.position);

        enemy2Movement.isEnemyMoving = true;


        if (enemy2Movement.hasMoved)
        {
            EndEnemy2Turn();
        }
    }

    public void EndEnemy2Turn()
    {
        Debug.Log("Koniec tury drugiego przeciwnika");
        currentTurn = (TurnState)(((int)currentTurn + 1) % 4);
        currentTurnIndex = (currentTurnIndex + 1) % characters.Length;
        UpdateTurnIndicator();
        isEnemyTurn = false;

        StartTurn();
    }

    public void UpdateTurnIndicator()
    {

        if (activeCharacters.Count == 0)
        {
            Debug.Log("Brak aktywnych postaci! Gra zako�czona.");
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
            case TurnState.Enemy1:
                turnImage.sprite = characterSprites[2];
                break;
            case TurnState.Enemy2:
                turnImage.sprite = characterSprites[3];
                break;
        }
    }

    public void OnCharacterDeath(GameObject deadCharacter)
    {
        if (activeCharacters.Contains(deadCharacter))
        {
            activeCharacters.Remove(deadCharacter);
            Debug.Log(deadCharacter.name + " zosta� usuni�ty z aktywnych postaci.");
        }
    }







    bool IsInAttackRange(Vector3 attackerPos, Vector3 targetPos) // OGARN�� TO W JAKIM� INNYM SKRYPCIE CZY CO�
    {
        // Pobierz pozycje heks�w na siatce Tilemap
        Vector3Int attackerHexPos = playerMovement.hexTilemap.WorldToCell(attackerPos);
        Vector3Int targetHexPos = playerMovement.hexTilemap.WorldToCell(targetPos);

        // Zamie� pozycje na obiekty Hex
        Hex attackerHex = new Hex(attackerHexPos.x, attackerHexPos.y);
        Hex targetHex = new Hex(targetHexPos.x, targetHexPos.y);

        // Sprawd�, czy target znajduje si� w s�siaduj�cych heksach
        foreach (var neighbor in attackerHex.GetAllNeighbors())
        {
            if (neighbor.q == targetHex.q && neighbor.r == targetHex.r)
            {
                return true; // Target jest w zasi�gu ataku
            }
        }

        return false; // Target nie jest w zasi�gu ataku
    }


}




