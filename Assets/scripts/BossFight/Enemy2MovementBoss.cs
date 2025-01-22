using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Enemy2MovementBoss : MonoBehaviour
{
    public Tilemap hexTilemap;
    public HexPathfindingForEnemy2Boss hexTilemapPathfinding;
    public PlayerMovementBoss playerMovement;
    public TurnManagerBoss turnManager;
    public TileManager tileManager;

    public bool isEnemyMoving = false;
    public bool isActive = false;
    public bool hasMoved = false;

    private float moveSpeed = 0.8f;
    private int movementRange = 3;

    private Vector3 targetPosition;
    private Vector3Int currentHexPosition;

    private List<Vector3Int> path1;
    private int currentPathIndex = 0;

    public Animator animator;


    void Start()
    {
        targetPosition = transform.position;
        Vector3Int startHex = tileManager.GetTilePosition(transform.position);
        currentHexPosition = hexTilemap.WorldToCell(transform.position);
        tileManager.OccupyTile(currentHexPosition, gameObject);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        path1 = hexTilemapPathfinding.GetPath();

        if (isEnemyMoving)
        {
            MoveEnemyAlongPath();
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    public void SetPath(List<Vector3Int> newPath)
    {
        path1 = newPath;
        currentPathIndex = 0; // Resetujemy indeks �cie�ki
        //Debug.Log("�cie�ka ustawiona: " + string.Join(", ", newPath.Select(p => p.ToString())));
    }

    public void MoveEnemyAlongPath()
    {
        if (path1 == null || path1.Count == 0)
        {
            //Debug.Log("Brak �cie�ki do przebycia.");
            EndEnemyMovement();
            return;
        }

        if (currentPathIndex >= path1.Count || currentPathIndex >= movementRange)
        {
            //Debug.Log($"Ruch zako�czony! Index: {currentPathIndex}, Range: {movementRange}, Path Count: {path1.Count}");
            EndEnemyMovement();
            return;
        }

        if (currentHexPosition == targetPosition)
        {
            //Debug.Log($"Przeciwnik {gameObject.name} osi�gn�� cel na {currentHexPosition}. Zatrzymanie.");
            isEnemyMoving = false;
            return;
        }

        if (currentPathIndex < path1.Count)
        {
            Vector3Int targetHexPosition = path1[currentPathIndex];
            Vector3 targetWorldPosition = hexTilemap.CellToWorld(targetHexPosition);

            if (tileManager.IsTileOccupied(targetHexPosition))
            {
                //Debug.LogWarning($"WWWWWWWWW Przeciwnik {gameObject.name} koliduje z innym przeciwnikiem na {targetHexPosition}. Generuj� now� �cie�k�.");

                Vector3Int alternativeHexPosition = FindAlternativeTile(targetHexPosition, path1[^1]); // path[^1] = cel ko�cowy

                if (alternativeHexPosition == Vector3Int.zero)
                {
                    //Debug.LogWarning($"Brak dost�pnych alternatywnych kafelk�w dla przeciwnika {gameObject.name}. Zatrzymanie ruchu.");
                    EndEnemyMovement();
                    return;
                }

                //Debug.Log($"Przeciwnik {gameObject.name} zmierza na alternatywny kafelek: {alternativeHexPosition}.");
                path1.Insert(currentPathIndex, alternativeHexPosition); // Dodajemy alternatywny kafelek do �cie�ki
                targetHexPosition = alternativeHexPosition;

            }

            transform.position = Vector3.MoveTowards(transform.position, targetWorldPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWorldPosition) < 0.05f)
            {
                transform.position = targetWorldPosition;

                if (gameObject != null)
                {
                    Vector3Int newHexPosition = hexTilemap.WorldToCell(transform.position);
                    //Debug.Log($"Enemy {gameObject.name} moved to {newHexPosition}. Previous position: {currentHexPosition}.");

                    tileManager.UpdateTileOccupation(currentHexPosition, newHexPosition, gameObject);
                    currentHexPosition = newHexPosition;

                    if (currentPathIndex == path1.Count - 1)
                    {
                        //Debug.Log($"Przeciwnik {gameObject.name} dotar� na alternatywny kafelek {newHexPosition}. Zatrzymanie ruchu.");
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

        if (currentPathIndex >= path1.Count || currentPathIndex >= movementRange)
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

                if (!tileManager.IsTileOccupied(neighbor)) // Je�li kafelek jest wolny
                {
                    //Debug.Log($"Znaleziono alternatywny kafelek {neighbor}.");
                    return neighbor;
                }

                openSet.Enqueue(neighbor); // Dodajemy s�siada do kolejki
            }
        }

        // Je�li nie znaleziono alternatywnego kafelka, zwracamy pozycj� pocz�tkow�
        //Debug.LogWarning($"Nie znaleziono alternatywnego kafelka dla {blockedTile}. Zwracanie Vector3Int.zero.");
        return Vector3Int.zero;
    }

    private List<Vector3Int> GetNeighbors(Vector3Int position)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        Vector3Int[] directions = new Vector3Int[]
        {
            new Vector3Int(1, 0, 0),   // Prawo
            new Vector3Int(-1, 0, 0),  // Lewo
            new Vector3Int(0, 1, 0),   // G�ra
            new Vector3Int(0, -1, 0),  // D�
            new Vector3Int(1, -1, 0),  // Prawy dolny
            new Vector3Int(-1, 1, 0)   // Lewy g�rny
        };

        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighbor = position + direction;
            if (hexTilemap.HasTile(neighbor)) // Sprawdzamy, czy kafelek jest dost�pny
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

}