using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using UnityEngine.UI;


public class TurnManagerBoss : MonoBehaviour
{
    public PlayerMovementBoss playerMovement;
    public EnemyMovementBoss enemyMovement;
    public Enemy2MovementBoss enemy2Movement;
    public BatMateMovementBoss batMateMovement;
    public BossMovement bossMovement;

    public PlayerAbilitiesBoss playerAbilities;
    public EnemyAbilitiesBoss enemyAbilities;
    public Enemy2AbilitiesBoss enemy2Abilities;
    public BatMateAbilitiesBoss batMateAbilities;
    public BossAbilities bossAbilities;

    public TileManager tileManager;
    public Tilemap hexTilemap;

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
        Enemy2,
        Boss
    }

    private void Start()
    {
        activeCharacters.Add(playerMovement.gameObject);
        activeCharacters.Add(batMateMovement.gameObject);
        activeCharacters.Add(enemyMovement.gameObject);
        activeCharacters.Add(enemy2Movement.gameObject);
        activeCharacters.Add(bossMovement.gameObject);

        EnemyCount = 3;

        currentTurn = TurnState.Player;
        UpdateTurnIndicator();
        StartTurn();
    }

    private void Update()
    {
        if (EnemyCount == 0)
        {
            //Debug.Log("Wszyscy przeciwnicy s¹ martwi");
            SceneManager.LoadSceneAsync(6);
        }
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
            currentTurn = (TurnState)(((int)currentTurn + 1) % 5);
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
                    bossMovement.isActive = false;
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
                    bossMovement.isActive = false;
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
                    bossMovement.isActive = false;
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
                    bossMovement.isActive = false;
                    StartEnemy2Turn();
                }
                else
                {
                    EndEnemy2Turn();
                }
                break;
            case TurnState.Boss:
                if (bossAbilities.isAlive)
                {
                    playerMovement.isActive = false;
                    batMateMovement.isActive = false;
                    enemyMovement.isActive = false;
                    enemy2Movement.isActive = false;
                    bossMovement.isActive = true;
                    StartBossTurn();
                }
                else
                {
                    EndBossTurn();
                }
                break;
        }
    }

    void NextTurn()
    {
        currentTurnIndex = (currentTurnIndex + 1) % activeCharacters.Count;
        currentTurn = (TurnState)(((int)currentTurn + 1) % 5);
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
        if (!playerMovement.hasMoved)
        {
            Debug.Log("Musisz siê poruszyæ, zanim zakoñczysz turê!");
            return;
        }

        Debug.Log("Koniec tury gracza");

        playerAbilities.UpdateStatusEffects();

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
        foreach (EnemyAbilitiesBoss enemy in EnemyAbilitiesBoss.activeEnemies)
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
        foreach (Enemy2AbilitiesBoss enemy in Enemy2AbilitiesBoss.activeEnemies)
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

    public void StartBossTurn()
    {
        isEnemyTurn = true;
        Debug.Log("Tura Bossa");
        bossMovement.ResetMovement();

        Vector3Int playerHexPos = playerMovement.hexTilemap.WorldToCell(playerMovement.transform.position); //Pobiera pozycje gracza i przeciwnika
        Vector3Int bossHexPos = bossMovement != null ? hexTilemap.WorldToCell(bossMovement.transform.position) : Vector3Int.zero;

        bossMovement.isEnemyMoving = true;

        bossAbilities.OnBossTurn();

        if (bossMovement.hasMoved)
        {
            EndBossTurn();
        }
        
    }

    public void EndBossTurn()
    {
        foreach (BossAbilities enemy in BossAbilities.activeEnemies)
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
            case TurnState.Boss:
                turnImage.sprite = characterSprites[4];
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

            if (deadCharacter.TryGetComponent<PlayerAbilitiesBoss>(out var playerAbilities))
            {
                playerAbilities.isAlive = false;
            }
            else if (deadCharacter.TryGetComponent<EnemyAbilitiesBoss>(out var enemyAbilities))
            {
                enemyAbilities.isAlive = false;
                EnemyCount--;
            }
            else if (deadCharacter.TryGetComponent<Enemy2AbilitiesBoss>(out var enemy2Abilities))
            {
                enemy2Abilities.isAlive = false;
                EnemyCount--;
            }
            else if (deadCharacter.TryGetComponent<BatMateAbilitiesBoss>(out var batMateAbilities))
            {
                batMateAbilities.isAlive = false;
            }
            else if (deadCharacter.TryGetComponent<BossAbilities>(out var bossAbilities))
            {
                bossAbilities.isAlive = false;
                EnemyCount--;
            }


            if (activeCharacters.Count > 0 && activeCharacters[currentTurnIndex] == deadCharacter)
            {
                currentTurnIndex %= activeCharacters.Count;
            }

            if (EnemyCount <= 0)
            {
                Debug.Log("Wszyscy przeciwnicy zginêli! Przechodzimy do nastêpnej sceny.");
                SceneManager.LoadScene(6);
            }
        }
    }



}




