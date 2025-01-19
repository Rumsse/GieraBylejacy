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
    public Button BasicAttackButton;
    public Button AdvancedAttackButton;

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
        audioManager.Buttons(audioManager.buttonClicked);

        isPlayerTurn = false;
        Debug.Log("Koniec tury gracza");

        enemyAbilities.hasTakenDamage = false;
        playerAbilities.isAttackMode1 = false;
        playerAbilities.isAttackMode2 = false;

        BasicAttackButton.GetComponent<Image>().color = Color.white;
        AdvancedAttackButton.GetComponent<Image>().color = Color.white;

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
