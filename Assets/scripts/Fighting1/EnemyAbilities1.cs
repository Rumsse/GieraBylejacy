using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAbilities1 : MonoBehaviour
{
    public GameObject player;
    public HealthBar1 enemyHealthBar;
    public PlayerAbilities1 playerAbilities;
    public EnemyMovement1 enemyMovement;
    public Pause pause;

    public int maxHealth = 50;
    public int currentHealth;
    public bool hasTakenDamage = false;

    public static List<EnemyAbilities1> activeEnemies = new List<EnemyAbilities1>();


    void Start()
    {
        currentHealth = maxHealth;

        EnemyAbilities1.activeEnemies.Add(this);
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();
        hasTakenDamage = true;

        if (currentHealth <= 0)
        {
            EnemyAbilities1.activeEnemies.Remove(this);
            Destroy(gameObject);
            Debug.Log("Przeciwnik zosta� pokonany!");
            pause.WinScreenPause();
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
