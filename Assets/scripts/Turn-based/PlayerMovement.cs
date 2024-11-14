using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.Tilemaps;


public class PlayerMovement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    public bool hasMoved = false;
    public float yOffset = 0.3f;
    public EnemyMovement enemy;

    public HealthBar playerHealthBar;
    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 20;

    public HealthBarFollow healthBarFollow;
    public Transform playerTransform;

    public int maxMoveRange = 2;
    public Tile highlightTile;
    public Tilemap HighlightTilemap;
    public GameObject highlightPrefab;
    private List<GameObject> activeHighlights = new List<GameObject>();



    void Start()
    {
        currentHealth = maxHealth;
        playerHealthBar.SetMaxHealth(maxHealth);
        UpdateHealthUI();

        healthBarFollow.target = playerTransform;

        if (hexTilemap == null)
        {
            hexTilemap = GameObject.Find("HexTilemap").GetComponent<Tilemap>();
        }
        targetPosition = transform.position;

        if (playerHealthBar == null)
        {
            playerHealthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBar>();
        }

        if (playerHealthBar != null)
        {
            UpdateHealthUI();
        }

    }

    void Update()
    {
        if (healthBarFollow != null && healthBarFollow.target != null)
        {
            // Ustaw pozycjê paska zdrowia nad g³ow¹ gracza
            Vector3 playerPosition = healthBarFollow.target.position;
            playerPosition.y += 0f;
            playerPosition.z = 0;
            healthBarFollow.transform.position = playerPosition;
        }

        if (!hasMoved)
        {
            HighlightMoveRange();
        }

        if (Input.GetKeyDown(KeyCode.Space) && hasMoved)
        {
            EndTurn();
        }

        if (Input.GetMouseButtonDown(0) && !hasMoved)  // Gracz mo¿e klikn¹æ tylko raz na turê
        {
            HandleMouseClick();
        }

        MovePlayerToTarget();

    }

    void HandleMouseClick()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        if (IsEnemyClicked(mouseWorldPos) && IsEnemyInAttackRange())
        {
            AttackEnemy();
        }
        else
        {
            Vector3Int hexPosition = hexTilemap.WorldToCell(mouseWorldPos);

            if (hexTilemap.HasTile(hexPosition))
            {
                Vector3 targetWorldPosition = hexTilemap.CellToWorld(hexPosition) + hexTilemap.tileAnchor;

                if (!IsPositionOccupiedByEnemy(targetWorldPosition) && IsWithinMoveRange(targetWorldPosition))
                {
                    targetPosition = targetWorldPosition;
                    hasMoved = true;
                    ClearHighlightedTiles();
                }
                else
                {
                    Debug.Log("Nie mo¿na poruszyæ siê na kafelek zajêty przez przeciwnika lub poza zasiêgiem.");
                }
            }
            else
            {
                Debug.LogWarning("Klikniêto poza heksagonaln¹ siatk¹.");
            }
        }
    }


    bool IsPositionOccupiedByEnemy(Vector3 position)
    {
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(enemy.transform.position);
        Vector3Int targetHexPos = hexTilemap.WorldToCell(position);
        return enemyHexPos == targetHexPos;
    }

    void MovePlayerToTarget()
    {
        // Jeœli gracz nie osi¹gn¹³ jeszcze docelowej pozycji
        if (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        // Kiedy gracz dotrze do celu, zakoñcz ruch
        if (transform.position == targetPosition && !hasMoved)
        {
            hasMoved = true; 
            Debug.Log("Gracz zakoñczy³ ruch");
            ClearHighlightedTiles(); 
        }
    }

    bool IsEnemyClicked(Vector3 mousePosition)
    {
        Collider2D hit = Physics2D.OverlapPoint(mousePosition);
        return hit != null && hit.transform == enemy.transform;
    }

    bool IsEnemyInAttackRange()
    {
        Vector3Int playerHexPos = hexTilemap.WorldToCell(transform.position);
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(enemy.transform.position);
        return HexDistance(playerHexPos, enemyHexPos) == 1;
    }

    void AttackEnemy()
    {
        enemy.TakeDamage(damage);
        Debug.Log("Atak na przeciwnika!");
    }

    bool IsWithinMoveRange(Vector3 position)
    {
        Vector3Int playerHexPos = hexTilemap.WorldToCell(transform.position);
        Vector3Int targetHexPos = hexTilemap.WorldToCell(position);
        return HexDistance(playerHexPos, targetHexPos) <= maxMoveRange;
    }

    void EndTurn()
    {
        hasMoved = false;
        Debug.Log("Tura gracza zakoñczona.");
        HighlightMoveRange();
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        playerHealthBar.SetHealth(currentHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Gracz zosta³ pokonany!");
        }
    }

    private void UpdateHealthUI()
    {
        playerHealthBar.SetHealth(currentHealth);
    }

    public void HighlightMovableTiles()
    {
        Vector3Int playerHexPos = hexTilemap.WorldToCell(transform.position);
        List<Vector3Int> reachableHexes = GetReachableHexes(playerHexPos, maxMoveRange);

        foreach (Vector3Int hexPos in reachableHexes)
        {
            HighlightTilemap.SetTile(hexPos, highlightTile); // Ustawienie kafelka podœwietlenia na osiagalnych heksach
        }
    }


    public List<Vector3Int> GetReachableHexes(Vector3Int startPos, int range)
    {
        List<Vector3Int> reachableHexes = new List<Vector3Int>();

        Queue<(Vector3Int, int)> hexQueue = new Queue<(Vector3Int, int)>();
        hexQueue.Enqueue((startPos, 0)); // Dodaj startowy heks z dystansem 0

        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        visited.Add(startPos);

        while (hexQueue.Count > 0)
        {
            var (currentHex, distance) = hexQueue.Dequeue();

            if (distance >= range)
                continue;

            foreach (Vector3Int neighbor in GetNeighboringHexes(currentHex))
            {
                if (!visited.Contains(neighbor) && hexTilemap.HasTile(neighbor))
                {
                    visited.Add(neighbor);
                    hexQueue.Enqueue((neighbor, distance + 1));
                    reachableHexes.Add(neighbor);
                }
            }
        }

        return reachableHexes;
    }

    List<Vector3Int> GetNeighboringHexes(Vector3Int hexPos)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>
        {
            new Vector3Int(hexPos.x + 1, hexPos.y, hexPos.z),
            new Vector3Int(hexPos.x - 1, hexPos.y, hexPos.z),
            new Vector3Int(hexPos.x, hexPos.y + 1, hexPos.z),
            new Vector3Int(hexPos.x, hexPos.y - 1, hexPos.z),
            new Vector3Int(hexPos.x + 1, hexPos.y - 1, hexPos.z),
            new Vector3Int(hexPos.x - 1, hexPos.y + 1, hexPos.z)
        };
        return neighbors;
    }


    void ClearHighlight()
    {
        foreach (var highlight in activeHighlights)
        {
            Destroy(highlight);
        }
        activeHighlights.Clear();
    }

    void ClearHighlightedTiles()
    {
        foreach (GameObject tile in activeHighlights)
        {
            Destroy(tile);
        }
        activeHighlights.Clear();
    }

    void HighlightMoveRange()
    {
        ClearHighlightedTiles();

        Vector3Int playerHexPos = hexTilemap.WorldToCell(transform.position);

        for (int dx = -maxMoveRange; dx <= maxMoveRange; dx++)
        {
            for (int dy = Mathf.Max(-maxMoveRange, -dx - maxMoveRange); dy <= Mathf.Min(maxMoveRange, -dx + maxMoveRange); dy++)
            {
                int dz = -dx - dy;
                Vector3Int hexPos = playerHexPos + new Vector3Int(dx, dy, dz);

                if (hexTilemap.HasTile(hexPos))
                {
                    Vector3 worldPos = hexTilemap.CellToWorld(hexPos) + hexTilemap.tileAnchor;
                    GameObject highlight = Instantiate(highlightPrefab, worldPos, Quaternion.identity);
                    activeHighlights.Add(highlight);
                }
            }
        }
    }

    int HexDistance(Vector3Int a, Vector3Int b)
    {
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.x + a.y - b.x - b.y)) / 2;
    }


}


