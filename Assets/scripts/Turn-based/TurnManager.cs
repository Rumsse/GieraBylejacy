using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurnManager : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public EnemyMovement enemyMovement;
    public PlayerAbilities playerAbilities;
    public EnemyAbilities enemyAbilities;
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

        // Rozpocznij Coroutine dla opóŸnienia przed ruchem przeciwnika
        StartCoroutine(StartEnemyTurnWithDelay());

        if (IsInAttackRange(playerMovement.transform.position, enemyMovement.transform.position))
        {
            enemyAbilities.TakeDamage(playerAbilities.damage);
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
        Debug.Log("Tura przeciwnika");
        enemyMovement.ResetMovement();
        enemyMovement.MoveTowardsPlayer();
    }

    void EndEnemyTurn()
    {
        isPlayerTurn = true;
        Debug.Log("Koniec tury przeciwnika");

        StartPlayerTurn();

        if (IsInAttackRange(enemyMovement.transform.position, playerMovement.transform.position))
        {
            playerAbilities.TakeDamage(enemyAbilities.damage);
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




