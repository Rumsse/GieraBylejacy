using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerAbilities : MonoBehaviour
{
    public EnemyAbilities enemyAbilities;
    public Enemy2Abilities enemy2Abilities;
    public GameObject enemy;
    public GameObject enemy2;
    public Tilemap hexTilemap;
    public TurnManager turnManager;
    public HealthBar playerHealthBar;

    public Animator animator;

    public Button BasicAttackButton;
    public Button AdvancedAttackButton;

    public bool isAttackMode1 = false;
    public bool isAttackMode2 = false;
    public bool canUseAdvancedAttack = true;

    private int maxHealth = 100;
    private int currentHealth;
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
            Debug.Log("Tryb ataku aktywny! Kliknij na przeciwnika, aby zaatakowaæ.");
        }
        else
        {
            BasicAttackButton.GetComponent<Image>().color = Color.white;
            Debug.Log("Tryb ataku wy³¹czony.");
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
        Vector3Int enemy2HexPos = hexTilemap.WorldToCell(enemy2.transform.position);


        if (hitCollider != null && hitCollider.CompareTag("Enemy"))
        {
            EnemyAbilities enemy = hitCollider.GetComponent<EnemyAbilities>();
            if (enemy != null && (HexDistance(playerHexPos, enemyHexPos) <= 1) && !enemy.hasTakenDamage)
            {
                animator.SetTrigger("Attack");
                enemy.TakeDamage(10);
                Debug.Log("Przeciwnik otrzyma³ obra¿enia: " + 10);
            }
            else
            {
                Debug.Log("Przeciwnik ju¿ otrzyma³ obra¿enia");
            }
        }
        else
        {
            Debug.Log("Promieñ nie trafi³ w przeciwnika.");
        }



        if (hitCollider != null && hitCollider.CompareTag("Enemy2"))
        {
            Enemy2Abilities enemy2 = hitCollider.GetComponent<Enemy2Abilities>();
            if (enemy2 != null && (HexDistance(playerHexPos, enemy2HexPos) <= 1) && !enemy2.hasTakenDamage)
            {
                animator.SetTrigger("Attack");
                enemy2.TakeDamage(10);
                Debug.Log("Przeciwnik otrzyma³ obra¿enia: " + 10);

            }
            else
            {
                Debug.Log("Przeciwnik ju¿ otrzyma³ obra¿enia");
            }
        }
        else
        {
            Debug.Log("Promieñ nie trafi³ w przeciwnika.");
        }

    }

    public void SelectAdvancedAttackMode()
    {
        isAttackMode2 ^= true;

        if (isAttackMode2)
        {
            AdvancedAttackButton.GetComponent<Image>().color = Color.red;
            Debug.Log("2Tryb ataku aktywny! Kliknij na przeciwnika, aby zaatakowaæ.");
        }
        else
        {
            AdvancedAttackButton.GetComponent<Image>().color = Color.white;
            Debug.Log("2Tryb ataku wy³¹czony.");
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
        Vector3Int enemy2HexPos = hexTilemap.WorldToCell(enemy2.transform.position);


        if (canUseAdvancedAttack)
        {
            if (hitCollider != null && hitCollider.CompareTag("Enemy"))
            {
                EnemyAbilities enemy = hitCollider.GetComponent<EnemyAbilities>();
                if (enemy != null && (HexDistance(playerHexPos, enemyHexPos) <= 2) && !enemy.hasTakenDamage)
                {
                    animator.SetTrigger("Attack");
                    enemy.TakeDamage(20);
                    Debug.Log("2Przeciwnik otrzyma³ obra¿enia: " + 20);

                    canUseAdvancedAttack = false;
                    turnManager.turnCounter = 4;
                }
                else
                {
                    Debug.Log("2Przeciwnik ju¿ otrzyma³ obra¿enia");
                }
            }
            else
            {
                Debug.Log("2Promieñ nie trafi³ w przeciwnika.");
            }


            if (hitCollider != null && hitCollider.CompareTag("Enemy2"))
            {
                Enemy2Abilities enemy2 = hitCollider.GetComponent<Enemy2Abilities>();
                if (enemy2 != null && (HexDistance(playerHexPos, enemy2HexPos) <= 2) && !enemy2.hasTakenDamage)
                {
                    animator.SetTrigger("Attack");
                    enemy2.TakeDamage(20);
                    Debug.Log("2Przeciwnik otrzyma³ obra¿enia: " + 20);

                    canUseAdvancedAttack = false;
                    turnManager.turnCounter = 4;
                }
                else
                {
                    Debug.Log("2Przeciwnik ju¿ otrzyma³ obra¿enia");
                }
            }
            else
            {
                Debug.Log("2Promieñ nie trafi³ w przeciwnika.");
            }
        }
        else
        {
            Debug.Log("Advance Attack jest niedostêpny w tej turze");
        }

    }

    public void OnOneTurnEnd()
    {
        if (turnManager.turnCounter > 0)
        {
            turnManager.turnCounter--;
        }

        if (turnManager.turnCounter == 0)
        {
            canUseAdvancedAttack = true;
            Debug.Log("Advanced Attack jest ju¿ dostêpny");
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
            Debug.Log("Gracz zosta³ pokonany!");
            SceneManager.LoadSceneAsync(3);
        }
    }


    public void UpdateHealthUI()
    {
        playerHealthBar.SetHealth(currentHealth);
    }

}
