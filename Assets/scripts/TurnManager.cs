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
    }

    void Update()
    {
        if (isPlayerTurn)
        {
            if (playerMovement.hasMoved)
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

    void EndPlayerTurn()
    {
        isPlayerTurn = false;
        Debug.Log("Koniec tury gracza");

        // Rozpocznij Coroutine dla op�nienia przed ruchem przeciwnika
        StartCoroutine(StartEnemyTurnWithDelay());

        if (IsInAttackRange(playerMovement.transform.position, enemyMovement.transform.position))
        {
            enemyMovement.TakeDamage(playerMovement.damage);
        }

    }

    System.Collections.IEnumerator StartEnemyTurnWithDelay()
    {
        yield return new WaitForSeconds(enemyMoveDelay);
        StartEnemyTurn();
    }

    void StartEnemyTurn()
    {
        enemyMovement.ResetMovement();
        enemyMovement.MoveTowardsPlayer();
        Debug.Log("Tura przeciwnika");
    }

    void EndEnemyTurn()
    {
        isPlayerTurn = true;
        Debug.Log("Koniec tury przeciwnika");

        StartPlayerTurn();

        if (IsInAttackRange(enemyMovement.transform.position, playerMovement.transform.position))
        {
            playerMovement.TakeDamage(enemyMovement.damage);
        }
    }

    bool IsInAttackRange(Vector3 attackerPos, Vector3 targetPos)
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




