using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerAbilities1 : MonoBehaviour
{
    public MainMenu mainMenu;
    public EnemyAbilities1 enemyAbilities;
    public GameObject enemy;
    public Tilemap hexTilemap;
    public TurnManager1 turnManager;
    public HealthBar healthBar;
    public CharactersData characterData;

    public Animator animator;
    public AudioManager audioManager;

    public bool isAttackMode1 = false;
    public bool isAttackMode2 = false;
    public Button BasicAttackButton;
    public Button AdvancedAttackButton;

    public int playerDamageBasic = 10;
    public int playerDamageAdvanced = 20;
    public bool canUseAdvancedAttack = true;




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
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(enemy.transform.position);


        if (hitCollider != null && hitCollider.CompareTag("Enemy"))
        {
            EnemyAbilities1 enemy = hitCollider.GetComponent<EnemyAbilities1>();
            if (enemy != null && (HexDistance(playerHexPos, enemyHexPos) <= 1) && !enemy.hasTakenDamage)
            {
                animator.SetBool("isAttacking", true);
                StartCoroutine(DelayedDamage(enemy, 10));
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
            audioManager.Buttons(audioManager.buttonClicked);
            Debug.Log("2Tryb ataku aktywny! Kliknij na przeciwnika, aby zaatakowaæ.");
        }
        else
        {
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


        if (canUseAdvancedAttack)
        {
            if (hitCollider != null && hitCollider.CompareTag("Enemy"))
            {
                EnemyAbilities1 enemy = hitCollider.GetComponent<EnemyAbilities1>();
                if (enemy != null && (HexDistance(playerHexPos, enemyHexPos) <= 2) && !enemy.hasTakenDamage)
                {
                    animator.SetBool("isAttacking", true);
                    StartCoroutine(DelayedDamage(enemy, 20));

                    canUseAdvancedAttack = false;
                    turnManager.turnCounter = 2;
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


    public void TakeDamage(int damage)
    {
        characterData.TakeDamage(damage);
        healthBar.UpdateHealthUI();

        if (characterData.health <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log("Gracz zosta³ pokonany!");
            SceneManager.LoadSceneAsync(7);
        }
    }


    IEnumerator DelayedDamage(EnemyAbilities1 enemy, int damage)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        if (enemy != null && !enemy.hasTakenDamage)
        {
            enemy.TakeDamage(damage); 
            Debug.Log("Przeciwnik otrzyma³ obra¿enia: " + damage);
        }

        animator.SetBool("isAttacking", false);

    }

}
