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

        if (currentHexPosition == targetPosition)
        {
            Debug.Log($"Przeciwnik {gameObject.name} osi¹gn¹³ cel na {currentHexPosition}. Zatrzymanie.");
            isEnemyMoving = false;
            return;
        }

        if (currentPathIndex < path.Count)
        {
            Vector3Int targetHexPosition = path[currentPathIndex];
            Vector3 targetWorldPosition = hexTilemap.CellToWorld(targetHexPosition);

            if (tileManager.IsTileOccupied(targetHexPosition))
            {
                Debug.LogWarning($"WWWWWWWWW Przeciwnik {gameObject.name} koliduje z innym przeciwnikiem na {targetHexPosition}. Generujê now¹ œcie¿kê.");

                Vector3Int alternativeHexPosition = FindAlternativeTile(targetHexPosition, path[^1]); // path[^1] = cel koñcowy

                if (alternativeHexPosition == Vector3Int.zero)
                {
                    Debug.LogWarning($"Brak dostêpnych alternatywnych kafelków dla przeciwnika {gameObject.name}. Zatrzymanie ruchu.");
                    EndEnemyMovement();
                    return;
                }

                Debug.Log($"Przeciwnik {gameObject.name} zmierza na alternatywny kafelek: {alternativeHexPosition}.");
                path.Insert(currentPathIndex, alternativeHexPosition); // Dodajemy alternatywny kafelek do œcie¿ki
                targetHexPosition = alternativeHexPosition;

            }

            transform.position = Vector3.MoveTowards(transform.position, targetWorldPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWorldPosition) < 0.05f)
            {
                transform.position = targetWorldPosition;

                if (gameObject != null)
                {
                    Vector3Int newHexPosition = hexTilemap.WorldToCell(transform.position);
                    Debug.Log($"Enemy {gameObject.name} moved to {newHexPosition}. Previous position: {currentHexPosition}.");

                    tileManager.UpdateTileOccupation(currentHexPosition, newHexPosition, gameObject);
                    currentHexPosition = newHexPosition;

                    if (currentPathIndex == path.Count - 1)
                    {
                        Debug.Log($"Przeciwnik {gameObject.name} dotar³ na alternatywny kafelek {newHexPosition}. Zatrzymanie ruchu.");
                        isEnemyMoving = false;
                        EndEnemyMovement();
                        return;
                    }

                    currentPathIndex++;
                }
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








    private Vector3Int FindAlternativeTile(Vector3Int blockedTile, Vector3Int targetTile)
    {
        Queue<Vector3Int> openSet = new Queue<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

        openSet.Enqueue(blockedTile);
        visited.Add(blockedTile);

        while (openSet.Count > 0)
        {
            Vector3Int current = openSet.Dequeue();

            foreach (Vector3Int neighbor in GetNeighbors(current))
            {
                if (visited.Contains(neighbor)) continue; // Pomijamy odwiedzone kafelki

                visited.Add(neighbor);

                if (!tileManager.IsTileOccupied(neighbor)) // Jeœli kafelek jest wolny
                {
                    Debug.Log($"Znaleziono alternatywny kafelek {neighbor}.");
                    return neighbor;
                }

                openSet.Enqueue(neighbor); // Dodajemy s¹siada do kolejki
            }
        }

        // Jeœli nie znaleziono alternatywnego kafelka, zwracamy pozycjê pocz¹tkow¹
        Debug.LogWarning($"Nie znaleziono alternatywnego kafelka dla {blockedTile}. Zwracanie Vector3Int.zero.");
        return Vector3Int.zero;
    }

    private List<Vector3Int> GetNeighbors(Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(1, 0, 0),   // Prawo
            new Vector3Int(-1, 0, 0),  // Lewo
            new Vector3Int(0, 1, 0),   // Góra
            new Vector3Int(0, -1, 0),  // Dó³
            new Vector3Int(1, -1, 0),  // Prawy dolny
            new Vector3Int(-1, 1, 0)   // Lewy górny
        };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighbor = position + direction;
            if (hexTilemap.HasTile(neighbor)) // Sprawdzamy, czy kafelek jest dostêpny
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

}