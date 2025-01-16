using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Enemy2Movement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public HexTilemapPathfindingForEnemy2 hexTilemapPathfinding;
    public PlayerMovement playerMovement;
    public Transform playerTransform;
    public Transform batMateTransform;
    public Transform enemy1Transform;
    public TurnManager turnManager;
    public TileManager tileManager;

    public bool isEnemyMoving = false;
    public bool isActive = false;
    public bool hasMoved = false;

    private float moveSpeed = 0.8f;
    private int movementRange = 3;

    private Vector3 targetPosition;
    private Vector3Int currentHexPosition;

    private List<Vector3Int> path;
    private int currentPathIndex = 0;



    void Start()
    {
        targetPosition = transform.position;
        Vector3Int startHex = tileManager.GetTilePosition(transform.position);
        currentHexPosition = hexTilemap.WorldToCell(transform.position);
        tileManager.OccupyTile(currentHexPosition, gameObject);
    }

    void Update()
    {
        path = hexTilemapPathfinding.GetPath();

        if (isEnemyMoving)
        {
            MoveEnemyAlongPath();
        }
    }

    public void SetPath(List<Vector3Int> newPath)
    {
        path = newPath;
        currentPathIndex = 0; // Resetujemy indeks œcie¿ki
        Debug.Log("Œcie¿ka ustawiona: " + string.Join(", ", newPath.Select(p => p.ToString())));
    }

    public void MoveEnemyAlongPath()
    {
        if (path == null || path.Count == 0)
        {
            Debug.Log("Brak œcie¿ki do przebycia.");
            EndEnemyMovement();
            return;
        }

        if (currentPathIndex >= path.Count || currentPathIndex >= movementRange)
        {
            Debug.Log($"Ruch zakoñczony! Index: {currentPathIndex}, Range: {movementRange}, Path Count: {path.Count}");
            EndEnemyMovement();
            return;
        }

        if (currentPathIndex < path.Count)
        {
            Vector3Int targetHexPosition = path[currentPathIndex];
            Vector3 targetWorldPosition = hexTilemap.CellToWorld(targetHexPosition);

            transform.position = Vector3.MoveTowards(transform.position, targetWorldPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWorldPosition) < 0.05f)
            {
                transform.position = targetWorldPosition;

                Vector3Int newHexPosition = hexTilemap.WorldToCell(transform.position);
                Debug.Log($"Enemy {gameObject.name} moved to {newHexPosition}. Previous position: {currentHexPosition}.");

                tileManager.UpdateTileOccupation(currentHexPosition, newHexPosition, gameObject);
                currentHexPosition = newHexPosition;

                currentPathIndex++;
            }
        }
        else
        {
            EndEnemyMovement();
        }

        if (currentPathIndex >= path.Count || currentPathIndex >= movementRange)
        {
            EndEnemyMovement();
        }
    }

    private void EndEnemyMovement()
    {
        isEnemyMoving = false;
        hasMoved = true;
        currentPathIndex = 0;
        ResetMovement();
        playerMovement.ResetMovement();
        turnManager.EndEnemyTurn();
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }
}