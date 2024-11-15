using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemyMovement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public Transform playerTransform;
    public Transform enemyTransform;
    public HexTilemapPathfinding hexTilemapPathfinding;

    public float moveSpeed = 2f;
    public float yOffset = 0.3f;
    public int maxMoveDistance = 3;
    public bool hasMoved = false;
    private bool isMoving = false;

    private Vector3 targetPosition;


    void Start()
    {
        if (hexTilemap == null)
        {
            hexTilemap = GameObject.Find("HexTilemap").GetComponent<Tilemap>();
        }

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        targetPosition = transform.position;

    }

    void Update()
    {
        if (isMoving)
        {
            hexTilemapPathfinding.MoveEnemyAlongPath();
        }
    }

    bool IsPositionOccupiedByPlayer(Vector3 position)
    {
        Vector3Int playerHexPos = hexTilemap.WorldToCell(playerTransform.position);
        Vector3Int targetHexPos = hexTilemap.WorldToCell(position);
        return playerHexPos == targetHexPos;  // Zwraca true, jeœli pozycja jest zajêta przez gracza
    }

    void MoveEnemyToTarget() //czy to jest potrzebne????
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isMoving = false;
            hasMoved = true;
            Debug.Log("Przeciwnik dotar³ do docelowej pozycji.");
        }
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }
}


