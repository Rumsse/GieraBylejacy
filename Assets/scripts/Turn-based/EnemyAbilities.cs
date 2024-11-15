using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAbilities : MonoBehaviour
{
    public HealthBar enemyHealthBar;
    public int maxHealth = 50;
    public int damage = 10;

    private int currentHealth;


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
