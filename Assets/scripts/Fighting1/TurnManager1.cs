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
