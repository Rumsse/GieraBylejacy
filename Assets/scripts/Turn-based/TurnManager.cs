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

    public Button BasicAttackButton;
    public Button AdvancedAttackButton;

    public bool isPlayerTurn = true;
    public bool isBatMateTurn = true;
    public float enemyMoveDelay = 1.3f;

    public HexTilemapPathfinding hexTilemapPathfinding;
    public Vector3Int startPosition;
    public Vector3Int targetPosition;


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
            if (isBatMateTurn)
            {
                if (batMateMovement.hasMoved && Input.GetKeyDown(KeyCode.Space))
                {
                    EndBatMateTurn();
                }
            }
            else
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

    void EndPlayerTurn()
    {
        isPlayerTurn = false;
        Debug.Log("Koniec tury gracza");
        enemyAbilities.hasTakenDamage = false;
        playerAbilities.isAttackMode1 = false;
        playerAbilities.isAttackMode2 = false;

        BasicAttackButton.GetComponent<Image>().color = Color.white;
        AdvancedAttackButton.GetComponent<Image>().color = Color.white;

        StartBatMateTurn();

    }

    void StartBatMateTurn()
    {
        isBatMateTurn = true;
        batMateMovement.ResetMovement();
        Debug.Log("Tura Bat Mate'a");
    }

    void EndBatMateTurn()
    {
        isBatMateTurn = false;
        Debug.Log("Koniec tury Bat Mate'a");
        enemyAbilities.hasTakenDamage = false;
        StartCoroutine(StartEnemyTurnWithDelay()); // Rozpocznij Coroutine dla op�nienia przed ruchem przeciwnika
    }

    System.Collections.IEnumerator StartEnemyTurnWithDelay()
    {
        yield return new WaitForSeconds(enemyMoveDelay);
        Debug.Log("Op�nienie zako�czone, zaczynam tur� przeciwnika.");
        StartEnemyTurn();
    }

    void StartEnemyTurn()
    {
        Debug.Log("Tura przeciwnika");

        Vector3Int playerHexPos = playerMovement.hexTilemap.WorldToCell(playerMovement.transform.position); //Pobiera pozycje gracza i przeciwnika
        Vector3Int enemyHexPos = enemyMovement.hexTilemap.WorldToCell(enemyMovement.transform.position);

        enemyMovement.isEnemyMoving = true;
    }

    public void EndEnemyTurn()
    {
        isPlayerTurn = true;
        Debug.Log("Koniec tury przeciwnika");

        StartPlayerTurn();
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




