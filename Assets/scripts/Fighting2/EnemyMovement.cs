using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemyMovement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public HexTilemapPathfinding hexTilemapPathfinding;
    public PlayerMovement playerMovement;
    public Transform playerTransform;
    public Transform batMateTransform;
    public Transform enemy2Transform;
    public TurnManager turnManager;
    public TileManager tileManager;

    public bool isEnemyMoving = false;
    public bool isActive = false;
    public bool hasMoved = false;
    private float moveSpeed = 0.8f;
    private int movementRange = 3;

    private Vector3 targetPosition;

    private List<Vector3Int> path;
    private int currentPathIndex = 0;



    void Start()
    {
        targetPosition = transform.position;
        Vector3Int startHex = tileManager.hexTilemap.WorldToCell(transform.position);
        tileManager.OccupyTile(startHex);
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
        if (path.Count == 0)
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
            Vector3Int targetHexPos = path[currentPathIndex];
            Vector3 targetPosition = hexTilemap.CellToWorld(path[currentPathIndex]) + new Vector3(0, 0, 0); // Korygujemy pozycjê

            Debug.DrawLine(targetPosition, targetPosition + Vector3.up * 0.5f, Color.red, 5f);
            Debug.DrawLine(transform.position, transform.position + Vector3.up * 0.5f, Color.blue, 5f);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            float epsilon = 0.05f; // Tolerancja
            if (Vector3.Distance(transform.position, targetPosition) <= epsilon)
            {
                transform.position = targetPosition; // Wymuszenie ustawienia dok³adnej pozycji w œrodku kafelka
                currentPathIndex++;

                if (currentPathIndex < path.Count)
                {
                    Vector3Int currentHexPosition = hexTilemap.WorldToCell(transform.position);
                    Vector3Int targetHexPosition = path[currentPathIndex];
                    tileManager.UpdateTileOccupation(currentHexPosition, targetHexPosition);
                }

                Debug.Log($"Ruch na kafelek: {currentPathIndex}, Pozosta³o: {path.Count - currentPathIndex}");
            }

        }
        else
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