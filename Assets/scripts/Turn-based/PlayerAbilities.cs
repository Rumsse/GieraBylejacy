using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    public MainMenu mainMenu;
    public EnemyAbilities enemyAbilities;
    public GameObject enemy;
    public Tilemap hexTilemap;

    public bool isAttackMode1 = false;
    public bool isAttackMode2 = false;
    public Button BasicAttackButton;
    public Button AdvancedAttackButton;
    public LayerMask EnemyLayer;

    public HealthBar playerHealthBar;
    public int maxHealth = 100;
    public int currentHealth;
    public int playerDamageBasic = 10;
    public int playerDamageAdvanced = 20;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isAttackMode1)
        {
            BasicAttack();
        }

        if (Input.GetMouseButtonDown(0) && isAttackMode2)
        {
            AdvancedAttack();
        }
    }

    public void SelectBasicAttackMode()
    {
        isAttackMode1 ^= true;

        if (isAttackMode1)
        {
            BasicAttackButton.GetComponent<Image>().color = Color.red;
            Debug.Log("Tryb ataku aktywny! Kliknij na przeciwnika, aby zaatakowa�.");
        }
        else
        {
            BasicAttackButton.GetComponent<Image>().color = Color.white;
            Debug.Log("Tryb ataku wy��czony.");
        }

    }

    public void BasicAttack()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        Vector3Int hexPosition = hexTilemap.WorldToCell(mouseWorldPos);

        Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);

        Vector3Int playerHexPos = hexTilemap.WorldToCell(transform.position);
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(enemy.transform.position);


        if (hitCollider != null && hitCollider.CompareTag("Enemy"))
        {
            EnemyAbilities enemy = hitCollider.GetComponent<EnemyAbilities>();
            if (enemy != null && (HexDistance(playerHexPos, enemyHexPos) <= 1) && !enemy.hasTakenDamage)
            {
                enemy.TakeDamage(10);
                Debug.Log("Przeciwnik otrzyma� obra�enia: " + 10);
            }
            else
            {
                Debug.Log("Przeciwnik ju� otrzyma� obra�enia");
            }
        }
        else
        {
            Debug.Log("Promie� nie trafi� w przeciwnika.");
        }

    }

    public void SelectAdvancedAttackMode()
    {
        isAttackMode2 ^= true;

        if (isAttackMode2)
        {
            AdvancedAttackButton.GetComponent<Image>().color = Color.red;
            Debug.Log("2Tryb ataku aktywny! Kliknij na przeciwnika, aby zaatakowa�.");
        }
        else
        {
            AdvancedAttackButton.GetComponent<Image>().color = Color.white;
            Debug.Log("2Tryb ataku wy��czony.");
        }

    }

    public void AdvancedAttack()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        Vector3Int hexPosition = hexTilemap.WorldToCell(mouseWorldPos);

        Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);

        Vector3Int playerHexPos = hexTilemap.WorldToCell(transform.position);
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(enemy.transform.position);


        if (hitCollider != null && hitCollider.CompareTag("Enemy"))
        {
            EnemyAbilities enemy = hitCollider.GetComponent<EnemyAbilities>();
            if (enemy != null && (HexDistance(playerHexPos, enemyHexPos) <= 2) && !enemy.hasTakenDamage)
            {
                enemy.TakeDamage(20);
                Debug.Log("2Przeciwnik otrzyma� obra�enia: " + 20);
            }
            else
            {
                Debug.Log("2Przeciwnik ju� otrzyma� obra�enia");
            }
        }
        else
        {
            Debug.Log("2Promie� nie trafi� w przeciwnika.");
        }

    }


    private int HexDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Gracz zosta� pokonany!");
            SceneManager.LoadSceneAsync(2);
        }
    }


    private void UpdateHealthUI()
    {
        playerHealthBar.SetHealth(currentHealth);
    }

}
