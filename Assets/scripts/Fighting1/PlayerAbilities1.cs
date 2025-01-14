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
    public Animator animator;

    public bool isAttackMode1 = false;
    public bool isAttackMode2 = false;
    public Button BasicAttackButton;
    public Button AdvancedAttackButton;
    public LayerMask EnemyLayer;

    public HealthBar1 playerHealthBar;
    public int maxHealth = 100;
    public int currentHealth;
    public int playerDamageBasic = 10;
    public int playerDamageAdvanced = 20;
    public bool canUseAdvancedAttack = true;


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
            EnemyAbilities1 enemy = hitCollider.GetComponent<EnemyAbilities1>();
        
            if (enemy != null && (HexDistance(playerHexPos, enemyHexPos) <= 1) && !enemy.hasTakenDamage)
            {
                animator.SetBool("IsAttacking", true);

                enemy.TakeDamage(10);
                Debug.Log("Przeciwnik otrzyma� obra�enia: " + 10);

                StartCoroutine(ResetAttackAnimation());
                
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
    
    private IEnumerator ResetAttackAnimation()
    {
    // Wait for the duration of the attack animation (adjust this time as needed)
    yield return new WaitForSeconds(0.5f);  // 0.5 seconds is an example; match it to your animation duration

    // Reset the IsAttacking parameter after the animation has finished
    animator.SetBool("IsAttacking", false);
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


        if (canUseAdvancedAttack)
        {
            if (hitCollider != null && hitCollider.CompareTag("Enemy"))
            {
                EnemyAbilities1 enemy = hitCollider.GetComponent<EnemyAbilities1>();
                if (enemy != null && (HexDistance(playerHexPos, enemyHexPos) <= 2) && !enemy.hasTakenDamage)
                {
                    enemy.TakeDamage(20);
                    Debug.Log("2Przeciwnik otrzyma� obra�enia: " + 20);

                    canUseAdvancedAttack = false;
                    turnManager.turnCounter = 2;
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
        else
        {
            Debug.Log("Advance Attack jest niedost�pny w tej turze");
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
            Debug.Log("Advanced Attack jest ju� dost�pny");
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
            SceneManager.LoadSceneAsync(3);
        }
    }


    private void UpdateHealthUI()
    {
        playerHealthBar.SetHealth(currentHealth);
    }

}
