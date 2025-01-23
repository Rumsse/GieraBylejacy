using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyAbilitiesBoss : MonoBehaviour
{
    public GameObject player;
    public GameObject batmate;
    public PlayerAbilitiesBoss playerAbilities;
    public PlayerMovementBoss playerMovement;
    public BatMateAbilitiesBoss batMateAbilities;
    public EnemyMovementBoss enemyMovement;
    public TurnManagerBoss turnManager;
    public Tilemap hexTilemap;
    public HealthBar healthBar;
    public CharactersData characterData;

    public Animator animator;

    public bool hasTakenDamage = false;
    public bool isAlive = true;
    public bool isInAttackRange;

    public static List<EnemyAbilitiesBoss> activeEnemies = new List<EnemyAbilitiesBoss>();


    private void Awake()
    {
        characterData.ResetData();
        healthBar.UpdateHealthUI();
    }

    void Start()
    {
        activeEnemies.Add(this);
        characterData.ResetData();
    }


    public void TakeDamage(int damage)
    {
        characterData.TakeDamage(damage);
        healthBar.UpdateHealthUI();
        hasTakenDamage = true;

        if (characterData.health <= 0)
        {
            turnManager.OnCharacterDeath(gameObject);
            activeEnemies.Remove(this);
            Debug.Log("Przeciwnik zosta� pokonany!");
            Destroy(gameObject);
        }
    }


    public void AttackPlayer()
    {
        Vector3Int playerHexPos = hexTilemap.WorldToCell(player.transform.position);
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(transform.position);
        Vector3Int batmateHexPos = hexTilemap.WorldToCell(batmate.transform.position);

        int distance = HexDistance(enemyHexPos, playerHexPos);
        if (distance <= 1)
        {
            int damage = 20;
            var playerCharacterData = player.GetComponent<PlayerAbilitiesBoss>().characterData;

            playerAbilities.TakeDamage(damage);
            Debug.Log($"Przeciwnik zadaje graczowi {damage} obra�e�! Dystans: {distance}");
            animator.SetTrigger("Attack");
        }

        int distance2 = HexDistance(enemyHexPos, batmateHexPos);
        if (distance2 <= 1)
        {
            int damage = 5;
            var batmateCharacterData = batmate.GetComponent<BatMateAbilitiesBoss>().characterData;

            batMateAbilities.TakeDamage(damage);
            Debug.Log($"Przeciwnik zadaje batmatowi {damage} obra�e�! Dystans: {distance}");
            animator.SetTrigger("Attack");
        }

    }

    private int HexDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }

}
