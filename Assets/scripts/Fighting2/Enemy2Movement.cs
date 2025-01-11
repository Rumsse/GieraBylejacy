using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Enemy2Movement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public Transform playerTransform;
    public Transform batMateTransform;
    public Transform enemy1Transform;
    public HexTilemapPathfindingForEnemy2 hexTilemapPathfinding;

    public bool hasMoved = false;
    public bool isEnemyMoving = false;
    public bool isActive = false;

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
        if (isEnemyMoving)
        {  
            hexTilemapPathfinding.MoveEnemyAlongPath();
        }
    }

    bool IsPositionOccupiedByPlayer(Vector3 position)
    {
        Vector3Int playerHexPos = hexTilemap.WorldToCell(playerTransform.position);
        Vector3Int batMateHexPos = hexTilemap.WorldToCell(batMateTransform.position);
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(enemy1Transform.position);
        Vector3Int targetHexPos = hexTilemap.WorldToCell(position);
        return playerHexPos == targetHexPos;  // Zwraca true, jeœli pozycja jest zajêta przez gracza
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }
}