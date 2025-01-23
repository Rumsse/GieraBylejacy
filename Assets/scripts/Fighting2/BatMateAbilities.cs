using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class BatMateAbilities : MonoBehaviour
{
    public Button BasicAttackButton;
    public HealthBar batMateHealthBar;
    public EnemyAbilities enemyAbilities;
    public Enemy2Abilities enemy2Abilities;
    public GameObject enemy;
    public GameObject enemy2;
    public Tilemap hexTilemap;
    public TurnManager turnManager;
    public HealthBar healthBar;
    public CharactersData characterData;

    public Animator animator;
    public AudioManager audioManager;

    public int batMateDamageBasic = 5;
    public bool isAlive = true;
    public bool isAttackMode1 = false;




    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isAttackMode1)
        {
            BasicAttack();
        }
    }

    public void SelectBasicAttackMode()
    {
        isAttackMode1 ^= true;

        if (isAttackMode1)
        {
            audioManager.Buttons(audioManager.buttonClicked);
            Debug.Log("Tryb ataku aktywny! Kliknij na przeciwnika, aby zaatakowaæ.");
        }
        else
        {
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
        Vector3Int enemyHexPos = enemy != null ? hexTilemap.WorldToCell(enemy.transform.position) : Vector3Int.zero;
        Vector3Int enemy2HexPos = enemy2 != null ? hexTilemap.WorldToCell(enemy2.transform.position) : Vector3Int.zero;


        if (hitCollider != null && hitCollider.CompareTag("Enemy"))
        {
            EnemyAbilities enemy = hitCollider.GetComponent<EnemyAbilities>();
            if (enemy != null && (HexDistance(playerHexPos, enemyHexPos) <= 1) && !enemy.hasTakenDamage)
            {
                animator.SetBool("isAttacking", true);
                StartCoroutine(DelayedDamage(enemy, 5));
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



        if (hitCollider != null && hitCollider.CompareTag("Enemy2"))
        {
            Enemy2Abilities enemy2 = hitCollider.GetComponent<Enemy2Abilities>();
            if (enemy2 != null && (HexDistance(playerHexPos, enemy2HexPos) <= 1) && !enemy2.hasTakenDamage)
            {
                animator.SetBool("isAttacking", true);
                StartCoroutine(DelayedDamage2(enemy2, 5));

            }
            else
            {
                //Debug.Log("Przeciwnik ju¿ otrzyma³ obra¿enia");
            }
        }
        else
        {
            //Debug.Log("Promieñ nie trafi³ w przeciwnika.");
        }

    }

    private int HexDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }

    public void TakeDamage(int damage)
    {
        characterData.TakeDamage(damage);
        healthBar.UpdateHealthUI();

        if (characterData.health <= 0)
        {
            turnManager.OnCharacterDeath(gameObject);
            gameObject.SetActive(false);
            Debug.Log("Bat Mate zosta³ pokonany!");
            isAlive = false;
        }

    }

    IEnumerator DelayedDamage(EnemyAbilities enemy, int damage)
    {
        yield return new WaitForSeconds(0.2f);

        if (enemy != null && !enemy.hasTakenDamage)
        {
            enemy.TakeDamage(damage);
            //Debug.Log("Przeciwnik otrzyma³ obra¿enia: " + damage);
        }

        animator.SetBool("isAttacking", false);

    }

    IEnumerator DelayedDamage2(Enemy2Abilities enemy, int damage)
    {
        yield return new WaitForSeconds(0.2f);

        if (enemy != null && !enemy.hasTakenDamage)
        {
            enemy.TakeDamage(damage);
            //Debug.Log("Przeciwnik otrzyma³ obra¿enia: " + damage);
        }

        animator.SetBool("isAttacking", false);

    }

}
