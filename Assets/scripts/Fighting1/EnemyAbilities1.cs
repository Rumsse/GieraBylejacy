using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilities1 : MonoBehaviour
{
    public GameObject player;
    public HealthBar enemyHealthBar;
    public PlayerAbilities playerAbilities;
    public EnemyMovement enemyMovement;
    public int maxHealth = 50;
    public int currentHealth;
    public bool hasTakenDamage = false;


    void Start()
    {
        currentHealth = maxHealth;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();
        hasTakenDamage = true;

        if (currentHealth <= 0)
        {
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
