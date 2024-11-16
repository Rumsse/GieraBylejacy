using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerAbilities : MonoBehaviour
{
    public MainMenu mainMenu;
    public EnemyAbilities enemyAbilities;
    public GameObject enemy;
    public Tilemap hexTilemap;

    public HealthBar playerHealthBar;
    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 20;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            Vector3Int playerHexPos = hexTilemap.WorldToCell(transform.position);
            Vector3Int enemyHexPos = hexTilemap.WorldToCell(enemy.transform.position);

            if (HexDistance(playerHexPos, enemyHexPos) <= 1) // Sprawdzenie, czy przeciwnik jest w zasiêgu
            {
                Attack();
            }
            else
            {
                Debug.Log("Przeciwnik poza zasiêgiem ataku!");
            }
        }
    }

    private int HexDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
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
            SceneManager.LoadSceneAsync(2);
        }
    }

    public void Attack()
    {
        if (enemy != null)
        {
            enemyAbilities.TakeDamage(damage);
        }
        else
        {
            Debug.Log("Brak przeciwnika do zaatakowania.");
        }
    }

    private void UpdateHealthUI()
    {
        playerHealthBar.SetHealth(currentHealth);
    }

}
