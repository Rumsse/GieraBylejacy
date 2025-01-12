using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemyMovement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public Transform playerTransform;
    public Transform batMateTransform;
    public Transform enemy2Transform;
    public HexTilemapPathfinding hexTilemapPathfinding;
    public TileManager tileManager;

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
        Vector3Int startHex = tileManager.hexTilemap.WorldToCell(transform.position);
        tileManager.OccupyTile(startHex);

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