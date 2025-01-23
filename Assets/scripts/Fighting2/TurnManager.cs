using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
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
    public TileManager tileManager;
    public Tilemap hexTilemap;
    public Pause pause;

    public AudioManager audioManager;

    public bool isEnemyTurn;
    public float enemyMoveDelay = 1.3f;
    public int EnemyCount;

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
        Enemy1,
        Enemy2
    }

    private void Start()
    {
        activeCharacters.Add(playerMovement.gameObject);
        activeCharacters.Add(batMateMovement.gameObject);
        activeCharacters.Add(enemyMovement.gameObject);
        activeCharacters.Add(enemy2Movement.gameObject);

        EnemyCount = 2;

        currentTurn = TurnState.Player;
        UpdateTurnIndicator();
        StartTurn();
    }

    private void Update()
    {
        if (EnemyCount == 0)
        {
            pause.WinScreenPause();
        }
    }


    void StartTurn()
    {
        if (activeCharacters.Count == 0)
        {
            return;
        }

        currentTurnIndex %= activeCharacters.Count;
        GameObject currentCharacter = activeCharacters[currentTurnIndex];

        if (currentCharacter == null)
        {
            NextTurn();
            return;
        }

        while (!activeCharacters[currentTurnIndex].activeSelf)
        {
            currentTurnIndex = (currentTurnIndex + 1) % activeCharacters.Count;
            currentTurn = (TurnState)(((int)currentTurn + 1) % 4);
        }

        switch (currentTurn)
        {
            case TurnState.Player:
                if (playerAbilities.isAlive)
                {
                    playerMovement.isActive = true;
                    batMateMovement.isActive = false;
                    enemyMovement.isActive = false;
                    enemy2Movement.isActive = false;
                    StartPlayerTurn();
                }
                else
                {
                    EndPlayerTurn();
                }
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

    void NextTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % activeCharacters.Count;
        currentTurn = (TurnState)(((int)currentTurn + 1) % 4);
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

    void EndPlayerTurn()
    {
        if (!playerMovement.hasMoved)
        {
            Debug.Log("Musisz siê poruszyæ, zanim zakoñczysz turê!");
            return;
        }

        audioManager.Buttons(audioManager.buttonClicked);
        Debug.Log("Koniec tury gracza");

        playerAbilities.isAttackMode1 = false;
        playerAbilities.isAttackMode2 = false;
        enemyAbilities.hasTakenDamage = false;
        enemy2Abilities.hasTakenDamage = false;

        playerAbilities.OnOneTurnEnd();

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
        if (!batMateMovement.hasMoved)
        {
            Debug.Log("Musisz siê poruszyæ, zanim zakoñczysz turê!");
            return;
        }

        Debug.Log("Koniec tury bat mate'a");
        batMateAbilities.isAttackMode1 = false;
        enemyAbilities.hasTakenDamage = false;
        enemy2Abilities.hasTakenDamage = false;

        NextTurn();

    }

    void StartEnemyTurn()
    {
        isEnemyTurn = true;
        Debug.Log("Tura przeciwnika");
        enemyMovement.ResetMovement();

        Vector3Int playerHexPos = playerMovement.hexTilemap.WorldToCell(playerMovement.transform.position); //Pobiera pozycje gracza i przeciwnika
        Vector3Int enemyHexPos = enemyMovement != null ? hexTilemap.WorldToCell(enemyMovement.transform.position) : Vector3Int.zero;

        enemyMovement.isEnemyMoving = true;


        if (enemyMovement.hasMoved)
        {
            EndEnemyTurn();
        }
    }

    public void EndEnemyTurn()
    {
        foreach (EnemyAbilities enemy in EnemyAbilities.activeEnemies)
        {
            if (enemy.isAlive)
            {
                enemy.AttackPlayer();
            }
        }

        Debug.Log("Koniec tury przeciwnika");
        isEnemyTurn = false;
        NextTurn();
    }

    public void StartEnemy2Turn()
    {
        isEnemyTurn = true;
        Debug.Log("Tura drugiego przeciwnika");
        enemy2Movement.ResetMovement();

        Vector3Int playerHexPos = playerMovement.hexTilemap.WorldToCell(playerMovement.transform.position); //Pobiera pozycje gracza i przeciwnika
        Vector3Int enemy2HexPos = enemy2Movement != null ? hexTilemap.WorldToCell(enemy2Movement.transform.position) : Vector3Int.zero;

        enemy2Movement.isEnemyMoving = true;


        if (enemy2Movement.hasMoved)
        {
            EndEnemy2Turn();
        }
    }

    public void EndEnemy2Turn()
    {
        foreach (Enemy2Abilities enemy in Enemy2Abilities.activeEnemies)
        {
            if (enemy.isAlive)
            {
                enemy.AttackPlayer();
            }
        }

        Debug.Log("Koniec tury drugiego przeciwnika");
        isEnemyTurn = false;
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
            Debug.Log(deadCharacter.name + " zosta³ usuniêty z aktywnych postaci.");

            Vector3Int currentHexPosition = hexTilemap.WorldToCell(deadCharacter.transform.position);
            tileManager.ReleaseTile(currentHexPosition);

            if (deadCharacter.TryGetComponent<PlayerAbilities>(out var playerAbilities))
            {
                playerAbilities.isAlive = false;
            }
            else if (deadCharacter.TryGetComponent<EnemyAbilities>(out var enemyAbilities))
            {
                enemyAbilities.isAlive = false;
                EnemyCount--;
            }
            else if (deadCharacter.TryGetComponent<Enemy2Abilities>(out var enemy2Abilities))
            {
                enemy2Abilities.isAlive = false;
                EnemyCount--; 
            }
            else if (deadCharacter.TryGetComponent<BatMateAbilities>(out var batMateAbilities))
            {
                batMateAbilities.isAlive = false;
            }

            if (activeCharacters.Count > 0 && activeCharacters[currentTurnIndex] == deadCharacter)
            {
                currentTurnIndex %= activeCharacters.Count;
            }
            else
            {
                currentTurnIndex = 0;
            }

            // Sprawdzenie, czy wszyscy przeciwnicy s¹ martwi
            if (EnemyCount <= 0)
            {
                Debug.Log("Wszyscy przeciwnicy zginêli! Przechodzimy do nastêpnej sceny.");
                SceneManager.LoadScene(4); 
            }
        }
    }



}




