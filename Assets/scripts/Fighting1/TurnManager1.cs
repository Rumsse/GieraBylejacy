using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static TurnManager;


public class TurnManager1 : MonoBehaviour
{
    public PlayerMovement1 playerMovement;
    public EnemyMovement1 enemyMovement;
    public PlayerAbilities1 playerAbilities;
    public EnemyAbilities1 enemyAbilities;

    public AudioManager audioManager;

    public bool isPlayerTurn = true;
    public float enemyMoveDelay = 0.01f;

    public HexTilemapPathfinding1 hexTilemapPathfinding;
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
            if (enemyMovement.hasMoved)
            {
                EndEnemyTurn();
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
        if (!playerMovement.hasMoved)
        {
            Debug.Log("Musisz siê poruszyæ, zanim zakoñczysz turê!");
            return;
        }

        audioManager.Buttons(audioManager.buttonClicked);

        isPlayerTurn = false;
        Debug.Log("Koniec tury gracza");

        enemyAbilities.hasTakenDamage = false;
        playerAbilities.isAttackMode1 = false;
        playerAbilities.isAttackMode2 = false;

        currentTurnIndex = (currentTurnIndex + 1) % characters.Length;
        UpdateTurnIndicator();

        playerAbilities.OnOneTurnEnd();

        StartEnemyTurn();
    }

    void StartEnemyTurn()
    {
        isPlayerTurn = false;
        Debug.Log("Tura przeciwnika");

        enemyMovement.isEnemyMoving = true;
    }

    public void EndEnemyTurn()
    {
        foreach (EnemyAbilities1 enemy in EnemyAbilities1.activeEnemies)
        {
            if (enemy != null )
            {
                enemy.AttackPlayer();
            }
        }

        isPlayerTurn = true;
        Debug.Log("Koniec tury przeciwnika");

        currentTurnIndex = (currentTurnIndex + 1) % characters.Length;
        UpdateTurnIndicator();

        StartPlayerTurn();
    }

    public void UpdateTurnIndicator()
    {
        turnImage.sprite = characterSprites[currentTurnIndex];
    }




}
