using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerMovement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    public bool hasMoved = false;
    public float yOffset = 0.2f;
    public EnemyMovement enemy;

    public HealthBar playerHealthBar;
    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 20;

    public HealthBarFollow healthBarFollow;
    public Transform playerTransform;



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
            // Ustaw pozycj� paska zdrowia nad g�ow� gracza
            Vector3 playerPosition = healthBarFollow.target.position;
            playerPosition.y += 0f; // Mo�esz doda� przesuni�cie w osi Y, by pasek by� wy�ej
            playerPosition.z = 0;
            healthBarFollow.transform.position = playerPosition;
        }

        if (Input.GetMouseButtonDown(0) && !hasMoved)  // Gracz mo�e klikn�� tylko raz na tur�
        {
            HandleMouseClick();
        }

        MovePlayerToTarget();

    }

    void HandleMouseClick()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3Int hexPosition = hexTilemap.WorldToCell(mouseWorldPos);

        if (hexTilemap.HasTile(hexPosition))
        {
            Vector3 targetWorldPosition = hexTilemap.CellToWorld(hexPosition) + hexTilemap.tileAnchor;

            if (!IsPositionOccupiedByEnemy(targetWorldPosition))  // Sprawd�, czy pozycja nie jest zaj�ta
            {
                targetPosition = targetWorldPosition;
                hasMoved = true;
            }
            else
            {
                Debug.Log("Nie mo�na poruszy� si� na kafelek zaj�ty przez przeciwnika.");
            }
        }
        else
        {
            Debug.LogWarning("Klikni�to poza heksagonaln� siatk�.");
        }
    }

    bool IsPositionOccupiedByEnemy(Vector3 position)
    {
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(enemy.transform.position);
        Vector3Int targetHexPos = hexTilemap.WorldToCell(position);
        return enemyHexPos == targetHexPos;  // Zwraca true, je�li pozycja jest zaj�ta przez przeciwnika
    }

    void MovePlayerToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition && hasMoved)
        {
            Debug.Log("Gracz zako�czy� ruch");
        }
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
            Debug.Log("Gracz zosta� pokonany!");
        }
    }

    private void UpdateHealthUI()
    {
    }


}


