using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexTilemapPathfinding1 : MonoBehaviour
{
    [SerializeField] private Tilemap hexTilemap;      // Tilemap zawieraj�cy heksagonalne kafelki
    [SerializeField] private Transform playerUnit;    // Jednostka gracza
    [SerializeField] private Transform enemyUnit;     // Jednostka przeciwnika
    [SerializeField] private int movementRange = 3;   // Zasi�g ruchu (3 heksy)
    [SerializeField] private float moveSpeed = 3f;    // Szybko�� poruszania si� jednostki

    [SerializeField] private EnemyMovement1 enemyMovement;
    [SerializeField] private PlayerMovement1 playerMovement;
    [SerializeField] private TurnManager1 turnManager;

    private List<Vector3Int> path = new List<Vector3Int>();  // Lista punkt�w �cie�ki
    private int currentPathIndex = 0;  // Indeks aktualnego punktu �cie�ki

    private Vector3Int lastPlayerPos; // Ostatnia zaktualizowana pozycja gracza
    private Vector3Int lastEnemyPos;



    private void Start()
    {
        // Na pocz�tku mo�esz ustawi�, gdzie znajduje si� gracz i przeciwnik
        Vector3Int startEnemyPos = hexTilemap.WorldToCell(enemyUnit.position);  // Pozycja startowa przeciwnika
        Vector3Int startPlayerPos = hexTilemap.WorldToCell(playerUnit.position);  // Pozycja gracza

        // Wyszukiwanie �cie�ki do gracza na pocz�tku gry
        path = FindPath(startEnemyPos, startPlayerPos);
    }

    private void Update()
    {

        if (enemyUnit != null)
        {
            Vector3Int currentPlayerPos = hexTilemap.WorldToCell(playerUnit.transform.position); // Zaktualizuj pozycje gracza i przeciwnika
            Vector3Int currentEnemyPos = hexTilemap.WorldToCell(enemyUnit.transform.position);

            if (!enemyMovement.isEnemyMoving && currentPlayerPos != lastPlayerPos) // Sprawd�, czy pozycja gracza si� zmieni�a
            {
                path = FindPath(currentEnemyPos, currentPlayerPos);
                lastPlayerPos = currentPlayerPos;
                Debug.Log("Nowa pozycja gracza w siatce: " + currentPlayerPos);
                Debug.Log("Zaktualizowano �cie�k�: " + string.Join(" -> ", path));
            }

            if (currentEnemyPos != lastEnemyPos) // Sprawd�, czy pozycja przeciwnika si� zmieni�a
            {
                lastEnemyPos = currentEnemyPos;
                Debug.Log("Nowa pozycja przeciwnika w siatce: " + currentEnemyPos);
            }


            if (enemyMovement.isEnemyMoving && path.Count > 0) // Przeciwnik porusza si� po �cie�ce, je�li jest czas na jego ruch
            {
                MoveEnemyAlongPath();
            }
        }
        
    }

    // Funkcja wyszukuj�ca �cie�k� do celu (gracza) z uwzgl�dnieniem zasi�gu (w�a�nie nie ma zasi�gu, trzeba doda�)
    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int target)
    {
        List<Vector3Int> openSet = new List<Vector3Int>();    // Wierzcho�ki do rozwa�enia
        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>(); // Wierzcho�ki ju� rozwa�one
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>(); // �cie�ka prowadz�ca do punktu

        openSet.Add(start);

        // Heurystyka - odleg�o�� Manhattanowa (powinna raczej by� euklidesowa)
        float Heuristic(Vector3Int position)
        {
            return Mathf.Abs(position.x - target.x) + Mathf.Abs(position.y - target.y);
        }

        // Koszt przej�cia do punktu
        float Cost(Vector3Int current, Vector3Int neighbor)
        {
            return 1f;  // Zak�adaj�c sta�y koszt przej�cia mi�dzy s�siednimi kafelkami
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
                // Rekonstrukcja �cie�ki
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

                Debug.Log("�cie�ka: " + string.Join(" -> ", totalPath));

                return totalPath;
            }

            openSet.Remove(current);
            closedSet.Add(current);

            // Sprawdzamy s�siad�w
            foreach (Vector3Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor)) continue;

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

        return new List<Vector3Int>();  // Brak �cie�ki
    }

    // Zwracanie s�siednich kafelk�w (w zale�no�ci od uk�adu heksagonalnego)
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

    // Funkcja do p�ynnego poruszania si� jednostki wzd�u� �cie�ki
    public void MoveEnemyAlongPath()
    {
        if (currentPathIndex >= path.Count || currentPathIndex >= movementRange)
        {
            Debug.Log($"Ruch zako�czony! Index: {currentPathIndex}, Range: {movementRange}, Path Count: {path.Count}");
            EndEnemyMovement();
            return;
        }

        if (currentPathIndex < path.Count)
        {
            Vector3 targetPosition = hexTilemap.CellToWorld(path[currentPathIndex]) + new Vector3(0, 0, 0); // Korygujemy pozycj�

            Debug.DrawLine(targetPosition, targetPosition + Vector3.up * 0.5f, Color.red, 5f); // Linia pionowa od kafelka
            Debug.DrawLine(enemyUnit.position, enemyUnit.position + Vector3.up * 0.5f, Color.blue, 5f);

            enemyUnit.position = Vector3.MoveTowards(enemyUnit.position, targetPosition, moveSpeed * Time.deltaTime);

            float epsilon = 0.05f; // Tolerancja
            if (Vector3.Distance(enemyUnit.position, targetPosition) <= epsilon)
            {
                // Wymuszenie ustawienia dok�adnej pozycji w �rodku kafelka
                enemyUnit.position = targetPosition;
                currentPathIndex++; // Nast�pny punkt �cie�ki
                Debug.Log($"Ruch na kafelek: {currentPathIndex}, Pozosta�o: {path.Count - currentPathIndex}");
            }

        }
        else
        {
            Debug.Log("sprawdzam");
            EndEnemyMovement();
        }
    }

    private void EndEnemyMovement()
    {
        enemyMovement.isEnemyMoving = false;
        enemyMovement.hasMoved = true;
        currentPathIndex = 0;
        enemyMovement.ResetMovement();
        playerMovement.ResetMovement();
        turnManager.EndEnemyTurn();
    }

}
