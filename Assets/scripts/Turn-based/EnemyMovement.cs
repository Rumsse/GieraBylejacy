using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemyMovement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public Transform playerTransform;
    public float moveSpeed = 2f;
    private Vector3 targetPosition;
    public bool hasMoved = false; 
    private bool isMoving = false;
    public float yOffset = 0.3f;
    public int maxMoveDistance = 3;  


    public HealthBar enemyHealthBar;
    public int maxHealth = 50;          
    public int damage = 10;

    public HealthBarFollow healthBarFollow;
    public Transform enemyTransform;

    private int currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
        healthBarFollow.target = playerTransform;
        enemyHealthBar.SetMaxHealth(maxHealth);

        if (hexTilemap == null)
        {
            hexTilemap = GameObject.Find("HexTilemap").GetComponent<Tilemap>();
        }

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        targetPosition = transform.position;

        if (enemyHealthBar == null)
        {
            enemyHealthBar = GameObject.Find("EnemyHealthBar").GetComponent<HealthBar>();
        }

        UpdateHealthUI();
    }

    void Update()
    {
        if (isMoving)
        {
            MoveEnemyToTarget();
        }
    }

    public void MoveTowardsPlayer()
    {
        Vector3Int playerHexPos = hexTilemap.WorldToCell(playerTransform.position);
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(transform.position);

        Vector3Int nextHexPos = GetNextStepTowards(playerHexPos, enemyHexPos);
        Vector3 targetWorldPosition = hexTilemap.CellToWorld(nextHexPos) + hexTilemap.tileAnchor + new Vector3(0, yOffset, 0);

        // Tworzymy obiekty Hex z pozycji przeciwnika i gracza
        Hex enemyHex = new Hex(enemyHexPos.x, enemyHexPos.y);
        Hex playerHex = new Hex(playerHexPos.x, playerHexPos.y);

        if (!IsPositionOccupiedByPlayer(targetWorldPosition))
        {
            targetPosition = targetWorldPosition;
            isMoving = true;  // Ustaw isMoving na true, aby rozpocz¹æ ruch w Update
            hasMoved = true;
            Debug.Log("Przeciwnik zmierza do pozycji: " + targetPosition);
        }
        else
        {
            Debug.Log("Nie mo¿na poruszyæ siê na kafelek zajêty przez gracza.");
        }
    }


    bool IsPositionOccupiedByPlayer(Vector3 position)
    {
        Vector3Int playerHexPos = hexTilemap.WorldToCell(playerTransform.position);
        Vector3Int targetHexPos = hexTilemap.WorldToCell(position);
        return playerHexPos == targetHexPos;  // Zwraca true, jeœli pozycja jest zajêta przez gracza
    }

    void MoveEnemyToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isMoving = false;
            hasMoved = true;
            Debug.Log("Przeciwnik dotar³ do docelowej pozycji.");
        }
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }

    Vector3Int GetNextStepTowards(Vector3Int target, Vector3Int start)
    {
        int dq = target.x - start.x;
        int dr = target.y - start.y;

        if (Mathf.Abs(dq) > Mathf.Abs(dr))
        {
            start.x += (dq > 0) ? 1 : -1;
        }
        else
        {
            start.y += (dr > 0) ? 1 : -1;
        }

        return start;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        enemyHealthBar.SetHealth(currentHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Przeciwnik zosta³ pokonany!");
        }
    }

    private void UpdateHealthUI()
    {
    }

}


