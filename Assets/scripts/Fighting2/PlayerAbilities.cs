using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
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
    public HealthBar healthBar;
    public CharactersData characterData;

    public Animator animator;
    public AudioManager audioManager;

    public Button BasicAttackButton;
    public Button AdvancedAttackButton;

    public bool isAttackMode1 = false;
    public bool isAttackMode2 = false;
    public bool canUseAdvancedAttack = true;
    public bool isAlive = true;

    public int playerDamageBasic = 10;
    public int playerDamageAdvanced = 20;



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
            //Debug.Log("Tryb ataku aktywny! Kliknij na przeciwnika, aby zaatakowa�.");
        }
        else
        {
            //Debug.Log("Tryb ataku wy��czony.");
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
                StartCoroutine(DelayedDamage(enemy, 10));
            }
            else
            {
                //Debug.Log("Przeciwnik ju� otrzyma� obra�enia");
            }
        }
        else
        {
            //Debug.Log("Promie� nie trafi� w przeciwnika.");
        }



        if (hitCollider != null && hitCollider.CompareTag("Enemy2"))
        {
            Enemy2Abilities enemy2 = hitCollider.GetComponent<Enemy2Abilities>();
            if (enemy2 != null && (HexDistance(playerHexPos, enemy2HexPos) <= 1) && !enemy2.hasTakenDamage)
            {
                animator.SetBool("isAttacking", true);
                StartCoroutine(DelayedDamage2(enemy2, 10));

            }
            else
            {
                //Debug.Log("Przeciwnik ju� otrzyma� obra�enia");
            }
        }
        else
        {
            //Debug.Log("Promie� nie trafi� w przeciwnika.");
        }

    }

    public void SelectAdvancedAttackMode()
    {
        isAttackMode2 ^= true;

        if (isAttackMode2)
        {
            audioManager.Buttons(audioManager.buttonClicked);
            //Debug.Log("2Tryb ataku aktywny! Kliknij na przeciwnika, aby zaatakowa�.");
        }
        else
        {
            //Debug.Log("2Tryb ataku wy��czony.");
        }

    }

    public void AdvancedAttack()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        Vector3Int hexPosition = hexTilemap.WorldToCell(mouseWorldPos);

        Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);

        Vector3Int playerHexPos = hexTilemap.WorldToCell(transform.position);
        Vector3Int enemyHexPos = enemy != null ? hexTilemap.WorldToCell(enemy.transform.position) : Vector3Int.zero;
        Vector3Int enemy2HexPos = enemy2 != null ? hexTilemap.WorldToCell(enemy2.transform.position) : Vector3Int.zero;


        if (canUseAdvancedAttack)
        {
            if (hitCollider != null && hitCollider.CompareTag("Enemy"))
            {
                EnemyAbilities enemy = hitCollider.GetComponent<EnemyAbilities>();
                if (enemy != null && (HexDistance(playerHexPos, enemyHexPos) <= 2) && !enemy.hasTakenDamage)
                {
                    animator.SetBool("isAttacking", true);
                    StartCoroutine(DelayedDamage(enemy, 20));

                    canUseAdvancedAttack = false;
                    turnManager.turnCounter = 4;
                }
                else
                {
                    //Debug.Log("2Przeciwnik ju� otrzyma� obra�enia");
                }
            }
            else
            {
                //Debug.Log("2Promie� nie trafi� w przeciwnika.");
            }


            if (hitCollider != null && hitCollider.CompareTag("Enemy2"))
            {
                Enemy2Abilities enemy2 = hitCollider.GetComponent<Enemy2Abilities>();
                if (enemy2 != null && (HexDistance(playerHexPos, enemy2HexPos) <= 2) && !enemy2.hasTakenDamage)
                {
                    animator.SetBool("isAttacking", true);
                    StartCoroutine(DelayedDamage2(enemy2, 20));

                    canUseAdvancedAttack = false;
                    turnManager.turnCounter = 4;
                }
                else
                {
                    //Debug.Log("2Przeciwnik ju� otrzyma� obra�enia");
                }
            }
            else
            {
                //Debug.Log("2Promie� nie trafi� w przeciwnika.");
            }
        }
        else
        {
            //Debug.Log("Advance Attack jest niedost�pny w tej turze");
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
            //Debug.Log("Advanced Attack jest ju� dost�pny");
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
            Debug.Log("Gracz zosta� pokonany!");
            SceneManager.LoadSceneAsync(6);
        }
    }

    IEnumerator DelayedDamage(EnemyAbilities enemy, int damage)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        if (enemy != null && !enemy.hasTakenDamage)
        {
            enemy.TakeDamage(damage);
            //Debug.Log("Przeciwnik otrzyma� obra�enia: " + damage);
        }

        animator.SetBool("isAttacking", false);

    }

    IEnumerator DelayedDamage2(Enemy2Abilities enemy, int damage)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        if (enemy != null && !enemy.hasTakenDamage)
        {
            enemy.TakeDamage(damage);
            //Debug.Log("Przeciwnik otrzyma� obra�enia: " + damage);
        }

        animator.SetBool("isAttacking", false);

    }

}
