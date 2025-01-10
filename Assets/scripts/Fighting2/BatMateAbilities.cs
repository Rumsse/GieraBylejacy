using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BatMateAbilities : MonoBehaviour
{
    public Button BasicAttackButton;
    public HealthBar batMateHealthBar;
    public GameObject enemy;
    public Tilemap hexTilemap;

    public int maxHealth = 20;
    public int currentHealth;
    public int batMateDamageBasic = 5;

    public bool isAttackMode1 = false;


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
    }

    void SelectBasicAttackMode()
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


        if (hitCollider != null && hitCollider.CompareTag("Enemy"))
        {
            EnemyAbilities enemy = hitCollider.GetComponent<EnemyAbilities>();
            if (enemy != null && (HexDistance(playerHexPos, enemyHexPos) <= 1) && !enemy.hasTakenDamage)
            {
                enemy.TakeDamage(5);
                Debug.Log("Przeciwnik otrzyma³ obra¿enia: " + 5);
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

    private int HexDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }

    public void TakeDamage()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Bat Mate zosta³ pokonany!");
        }
    }

    private void UpdateHealthUI()
    {
        batMateHealthBar.SetHealth(currentHealth);
    }
}
