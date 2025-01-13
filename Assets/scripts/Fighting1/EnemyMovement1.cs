using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemyMovement1 : MonoBehaviour
{
    public Tilemap hexTilemap;
    public Transform playerTransform;
    public Transform enemyTransform;
    public HexTilemapPathfinding1 hexTilemapPathfinding;

    public float moveSpeed = 2f;
    public float yOffset = 0.3f;
    public int maxMoveDistance = 3;
    public bool hasMoved = false;
    public bool isEnemyMoving = false;

    private Vector3 targetPosition;


    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        if (isEnemyMoving)
        {
            hexTilemapPathfinding.MoveEnemyAlongPath();
        }
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }
}
