using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy2Abilities : MonoBehaviour
{
    public GameObject player;
    public GameObject batmate;
    public HealthBar enemyHealthBar;
    public PlayerAbilities playerAbilities;
    public PlayerMovement playerMovement;
    public BatMateAbilities batMateAbilities;
    public Enemy2Movement enemyMovement;
    public TurnManager turnManager;
    public Tilemap hexTilemap;
    public HealthBar healthBar;
    public CharactersData characterData;

    public bool hasTakenDamage = false;
    public bool isAlive = true;

    public static List<Enemy2Abilities> activeEnemies = new List<Enemy2Abilities>();


    void Start()
    {
        Enemy2Abilities.activeEnemies.Add(this);
    }


    public void TakeDamage(int damage)
    {
        characterData.TakeDamage(damage);
        healthBar.UpdateHealthUI();
        hasTakenDamage = true;

        if (characterData.health <= 0)
        {
            turnManager.OnCharacterDeath(gameObject);
            Enemy2Abilities.activeEnemies.Remove(this);
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
            int damageAmount = 10;
            //playerAbilities.TakeDamage(damageAmount);
            Debug.Log($"Przeciwnik zadaje graczowi {damageAmount} obra¿eñ! Dystans: {distance}");
        }

        int distance2 = HexDistance(enemyHexPos, batmateHexPos);
        if (distance2 <= 1)
        {
            int damage = 5;
            var batmateCharacterData = batmate.GetComponent<BatMateAbilities>().characterData;

            batMateAbilities.TakeDamage(damage);
            Debug.Log($"Przeciwnik zadaje batmatowi {damage} obra¿eñ! Dystans: {distance}");
        }

    }

    private int HexDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }

}
