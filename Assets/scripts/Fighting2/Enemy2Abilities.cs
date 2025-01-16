using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Abilities : MonoBehaviour
{
    public GameObject player;
    public HealthBar enemyHealthBar;
    public PlayerAbilities playerAbilities;
    public Enemy2Movement enemyMovement;
    public TurnManager turnManager;

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
        if (enemyMovement.hasMoved)
        {

        }
    }

    //private bool IsPlayerNearby()
    //{
    //float distance = Vector3.Distance(transform.position, player.position);
    //return distance <= 1.1f;
    //}

    private int HexDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }

}
