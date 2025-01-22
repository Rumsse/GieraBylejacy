using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossAbilities : MonoBehaviour
{
    public GameObject player; 
    public PlayerAbilitiesBoss playerAbilities;

    public int TurnsBetweenDebuff = 3;
    public int currentTurnCooldown = 0;

    public void OnBossTurn()
    {
        if (!isAlive)
        {
            return; 
        }

        currentTurnCooldown++;

        if (currentTurnCooldown >= TurnsBetweenDebuff)
        {
            ApplyDebuff();
            currentTurnCooldown = 0;
        }
    }

    public void ApplyDebuff()
    {
        StatusEffect debuff = new StatusEffect("Os³abienie", 2, playerAbilities.playerDamageBasic / 2, playerAbilities.playerDamageAdvanced / 2, -2); //nazwa, czas trwania w turach, modyfikator ataku (0.5 = 50%), zmniejszenie ruchu
        playerAbilities.ApplyStatus(debuff);
        Debug.Log("Boss rzuci³ os³abienie na gracza");
    }




    public GameObject batmate;
    public PlayerMovementBoss playerMovement;
    public BatMateAbilitiesBoss batMateAbilities;
    public BossMovement enemyMovement;
    public TurnManagerBoss turnManager;
    public Tilemap hexTilemap;
    public HealthBar healthBar;
    public CharactersData characterData;

    public bool hasTakenDamage = false;
    public bool isAlive = true;
    public bool isInAttackRange;

    public static List<BossAbilities> activeEnemies = new List<BossAbilities>();


    void Start()
    {
        activeEnemies.Add(this);
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
            Debug.Log("Przeciwnik zosta³ pokonany!");
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
            int damage = 10;
            var playerCharacterData = player.GetComponent<PlayerAbilitiesBoss>().characterData;

            playerAbilities.TakeDamage(damage);
            Debug.Log($"Przeciwnik zadaje graczowi {damage} obra¿eñ! Dystans: {distance}");
        }

        int distance2 = HexDistance(enemyHexPos, batmateHexPos);
        if (distance2 <= 1)
        {
            int damage = 5;
            var batmateCharacterData = batmate.GetComponent<BatMateAbilitiesBoss>().characterData;

            batMateAbilities.TakeDamage(damage);
            Debug.Log($"Przeciwnik zadaje batmatowi {damage} obra¿eñ! Dystans: {distance}");
        }

    }

    private int HexDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }


}
