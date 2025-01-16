using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilities : MonoBehaviour
{
    public GameObject player;
    public HealthBar enemyHealthBar;
    public PlayerAbilities playerAbilities;
    public EnemyMovement enemyMovement;
    public TurnManager turnManager;

    private int maxHealth = 50;
    private int currentHealth;
    public bool hasTakenDamage = false;
    public bool isAlive = true;

    public static List<EnemyAbilities> activeEnemies = new List<EnemyAbilities>();


    void Start()
    {
        currentHealth = maxHealth;
        EnemyAbilities.activeEnemies.Add(this);
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
            EnemyAbilities.activeEnemies.Remove(this);
            Destroy(gameObject);
            Debug.Log("Przeciwnik zosta³ pokonany!");
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
