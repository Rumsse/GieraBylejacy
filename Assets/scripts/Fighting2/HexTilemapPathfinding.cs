using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexTilemapPathfinding : MonoBehaviour
{
    [SerializeField] private Tilemap hexTilemap;      // Tilemap zawieraj¹cy heksagonalne kafelki
    [SerializeField] private Transform playerUnit;    // Jednostka gracza
    [SerializeField] private Transform enemyUnit;     // Jednostka przeciwnika
    [SerializeField] private int movementRange = 3;   // Zasiêg ruchu (3 heksy)

    public EnemyMovement enemyMovement;
    public PlayerMovement playerMovement;
    public TurnManager turnManager;
    public TileManager tileManager;

    private List<Vector3Int> path = new List<Vector3Int>();  // Lista punktów œcie¿ki

    private Vector3Int lastPlayerPos; // Ostatnia zaktualizowana pozycja gracza
    private Vector3Int lastEnemyPos;



    private void Start()
    {
        // Na pocz¹tku mo¿esz ustawiæ, gdzie znajduje siê gracz i przeciwnik
        Vector3Int startEnemyPos = hexTilemap.WorldToCell(enemyUnit.position);  // Pozycja startowa przeciwnika
        Vector3Int startPlayerPos = hexTilemap.WorldToCell(playerUnit.position);  // Pozycja gracza

        // Wyszukiwanie œcie¿ki do gracza na pocz¹tku gry
        path = FindPath(startEnemyPos, startPlayerPos);
        enemyMovement.SetPath(path);  // Inicjalizowanie œcie¿ki w EnemyMovement
    }

    private void Update()
    {
        if (enemyUnit != null)
        {
            Vector3Int currentPlayerPos = hexTilemap.WorldToCell(playerUnit.transform.position); // Zaktualizuj pozycje gracza i przeciwnika
            Vector3Int currentEnemyPos = hexTilemap.WorldToCell(enemyUnit.transform.position);

            if (!enemyMovement.isEnemyMoving && currentPlayerPos != lastPlayerPos) // SprawdŸ, czy pozycja gracza siê zmieni³a
            {
                path = FindPath(currentEnemyPos, currentPlayerPos);
                Debug.Log("GGGGPrzeciwnik1 œcie¿ka: " + string.Join(" -> ", path));
                lastPlayerPos = currentPlayerPos;
                Debug.Log("Nowa pozycja gracza w siatce: " + currentPlayerPos);
                Debug.Log("Zaktualizowano œcie¿kê: " + string.Join(" -> ", path));
            }

            if (currentEnemyPos != lastEnemyPos) // SprawdŸ, czy pozycja przeciwnika siê zmieni³a
            {
                lastEnemyPos = currentEnemyPos;
                Debug.Log("Nowa pozycja przeciwnika w siatce: " + currentEnemyPos);
            }


            if (enemyMovement.isEnemyMoving && path.Count > 0) // Przeciwnik porusza siê po œcie¿ce, jeœli jest czas na jego ruch
            {
                enemyMovement.MoveEnemyAlongPath();
            }
        }
    }

    // Funkcja wyszukuj¹ca œcie¿kê do celu (gracza) z uwzglêdnieniem zasiêgu (w³aœnie nie ma zasiêgu, trzeba dodaæ)
    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int target)
    {
        List<Vector3Int> openSet = new List<Vector3Int>();    // Wierzcho³ki do rozwa¿enia
        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>(); // Wierzcho³ki ju¿ rozwa¿one
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>(); // Œcie¿ka prowadz¹ca do punktu

        openSet.Add(start);

        // Heurystyka - odleg³oœæ Manhattanowa (powinna raczej byæ euklidesowa)
        float Heuristic(Vector3Int position)
        {
            return Mathf.Abs(position.x - target.x) + Mathf.Abs(position.y - target.y);
        }

        // Koszt przejœcia do punktu
        float Cost(Vector3Int current, Vector3Int neighbor)
        {
            return 1f;  // Zak³adaj¹c sta³y koszt przejœcia miêdzy s¹siednimi kafelkami
        }

        Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float>();
        gScore[start] = 0;

        Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float>();
        fScore[start] = Heuristic(start);

        while (openSet.Count > 0)
        {
            // Zbieramy punkt z najmniejszym fScore
            Vector3Int current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (fScore[openSet[i]] < fScore[current])
                {
                    current = openSet[i];
                }
            }

            if (current == target)
            {
                // Rekonstrukcja œcie¿ki
                List<Vector3Int> totalPath = new List<Vector3Int>();
                while (cameFrom.ContainsKey(current))
                {
                    totalPath.Add(current);
                    current = cameFrom[current];
                }
                totalPath.Reverse();

                if (totalPath.Count > movementRange)
                {
                    totalPath = totalPath.GetRange(0, movementRange);
                }

                Debug.Log("Œcie¿ka: " + string.Join(" -> ", totalPath));

                path = totalPath;
                return totalPath;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            // Sprawdzamy s¹siadów
            foreach (Vector3Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor) || tileManager.IsTileOccupied(neighbor)) continue;

                float tentativeGScore = gScore[current] + Cost(current, neighbor);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGScore >= gScore.GetValueOrDefault(neighbor, float.MaxValue))
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
                fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor);
            }
        }

        return new List<Vector3Int>();  // Brak œcie¿ki
    }

    public List<Vector3Int> GetPath()
    {
        return path;
    }

    // Zwracanie s¹siednich kafelków (w zale¿noœci od uk³adu heksagonalnego)
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
