using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class EnemyAbilities1 : MonoBehaviour
{
    public GameObject player;
    public HealthBar1 enemyHealthBar;
    public PlayerAbilities1 playerAbilities;
    public PlayerMovement1 playerMovement;
    public EnemyMovement1 enemyMovement;
    public Pause pause;
    public Tilemap hexTilemap;

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
            Debug.Log("Przeciwnik zosta³ pokonany!");
            pause.WinScreenPause();
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
