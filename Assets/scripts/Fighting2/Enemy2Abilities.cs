using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy2Abilities : MonoBehaviour
{
    public GameObject player;
    public HealthBar enemyHealthBar;
    public PlayerAbilities playerAbilities;
    public PlayerMovement playerMovement;
    public Enemy2Movement enemyMovement;
    public TurnManager turnManager;
    public Tilemap hexTilemap;

    private int maxHealth = 50;
    private int currentHealth;
    public bool hasTakenDamage = false;
    public bool isAlive = true;

    public static List<Enemy2Abilities> activeEnemies = new List<Enemy2Abilities>();


    void Start()
    {
        currentHealth = maxHealth;
        Enemy2Abilities.activeEnemies.Add(this);
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();
        hasTakenDamage = true;

        if (currentHealth <= 0)
        {
            isAlive = false;
            turnManager.OnCharacterDeath(gameObject);
            Enemy2Abilities.activeEnemies.Remove(this);
            Destroy(gameObject);
            Debug.Log("Przeciwnik nr2 zosta³ pokonany!");
        }
    }

    private void UpdateHealthUI()
    {
        enemyHealthBar.SetHealth(currentHealth);
    }

    public void AttackPlayer()
    {
        Vector3Int playerHexPos = hexTilemap.WorldToCell(player.transform.position);
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(transform.position);

        int distance = HexDistance(enemyHexPos, playerHexPos);

        if (distance <= 1)
        {
            int damageAmount = 10;
            playerAbilities.TakeDamage(damageAmount);
            Debug.Log($"Przeciwnik zadaje graczowi {damageAmount} obra¿eñ! Dystans: {distance}");
        }

    }

    private int HexDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }

}
