using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurnManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public EnemyMovement enemyMovement;
    private bool isPlayerTurn = true;
    public float enemyMoveDelay = 1.3f;

    void Start()
    {
        if (playerMovement == null)
        {
            playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        }

        if (enemyMovement == null)
        {
            enemyMovement = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyMovement>();
        }

        StartPlayerTurn();
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
        playerMovement.HighlightMovableTiles();
        Debug.Log("Tura gracza");
    }

    void EndPlayerTurn()
    {

        if (playerMovement.hasMoved)
        {
            if (IsInAttackRange(playerMovement.transform.position, enemyMovement.transform.position))
            {
                enemyMovement.TakeDamage(playerMovement.damage);
            }

            isPlayerTurn = false; 
            Debug.Log("Koniec tury gracza");

            StartCoroutine(StartEnemyTurnWithDelay());
        }

    }

    System.Collections.IEnumerator StartEnemyTurnWithDelay()
    {
        yield return new WaitForSeconds(enemyMoveDelay);
        Debug.Log("OpóŸnienie zakoñczone, zaczynam turê przeciwnika.");
        StartEnemyTurn();
    }

    void StartEnemyTurn()
    {
        isPlayerTurn = false;
        enemyMovement.ResetMovement();
        Debug.Log("Tura przeciwnika");
        enemyMovement.MoveTowardsPlayer();
    }

    void EndEnemyTurn()
    {
        if (enemyMovement.hasMoved)
        {
            isPlayerTurn = true;
            Debug.Log("Koniec tury przeciwnika");

            if (IsInAttackRange(enemyMovement.transform.position, playerMovement.transform.position))
            {
                playerMovement.TakeDamage(enemyMovement.damage);
            }

            StartPlayerTurn();
        }

    }

    bool IsInAttackRange(Vector3 attackerPos, Vector3 targetPos)
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




