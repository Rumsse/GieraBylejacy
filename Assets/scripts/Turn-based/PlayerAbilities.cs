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

    public bool isAttackMode = false;
    public Button BasicAttackButton;
    public LayerMask EnemyLayer;

    public HealthBar playerHealthBar;
    public int maxHealth = 100;
    public int currentHealth;
    public int damage = 20;


    private void Start()
    {
        currentHealth = maxHealth;
        BasicAttackButton.onClick.AddListener(SelectAttackMode); 
    }

    void Update()
    {
        if (isAttackMode && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue, 3f);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("EnemyLayer")))
            {
                Debug.Log("Trafiono: " + hit.collider.name);
                EnemyAbilities enemy = hit.collider.GetComponent<EnemyAbilities>();
                if (enemy != null)
                {
                    Debug.Log("Co�");
                    enemy.TakeDamage(damage);
                    Debug.Log("Przeciwnik otrzyma� " + damage + " obra�e�!");
                    isAttackMode = false;
                }
            }
            else
            {
                Debug.Log("Promie� nie trafi� w przeciwnika.");
                Debug.Log("Ray origin: " + ray.origin);  // Dodaj wi�cej informacji o pocz�tku promienia
                Debug.Log("Ray direction: " + ray.direction);
            }
        }

    }

    public void SelectAttackMode()
    {
        Vector3Int playerHexPos = hexTilemap.WorldToCell(transform.position);
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(enemy.transform.position);

        if (HexDistance(playerHexPos, enemyHexPos) <= 1)
        {
            isAttackMode = true;
            BasicAttackButton.GetComponent<Image>().color = Color.red;
            Debug.Log("Tryb ataku aktywny! Kliknij na przeciwnika, aby zaatakowa�.");
        }
        else
        {
            isAttackMode = false;
            BasicAttackButton.GetComponent<Image>().color = Color.white;
            Debug.Log("Przeciwnik poza zasi�giem ataku!");
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
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Gracz zosta� pokonany!");
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
