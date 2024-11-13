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

        // Rozpocznij Coroutine dla opóŸnienia przed ruchem przeciwnika
        StartCoroutine(StartEnemyTurnWithDelay());
    }

    System.Collections.IEnumerator StartEnemyTurnWithDelay()
    {
        // Czekaj przez okreœlon¹ liczbê sekund
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
    }

}




