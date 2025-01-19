using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class EnemyAbilities1 : MonoBehaviour
{
    public GameObject player;
    public PlayerAbilities1 playerAbilities;
    public PlayerMovement1 playerMovement;
    public EnemyMovement1 enemyMovement;
    public Pause pause;
    public Tilemap hexTilemap;
    public HealthBar healthBar;
    public CharactersData characterData;

    public bool hasTakenDamage = false;

    public static List<EnemyAbilities1> activeEnemies = new List<EnemyAbilities1>();


    void Start()
    {
        EnemyAbilities1.activeEnemies.Add(this);
    }


    public void TakeDamage(int damage)
    {
        characterData.TakeDamage(damage);
        healthBar.UpdateHealthUI();
        hasTakenDamage = true;

        if (characterData.health <= 0)
        {
            EnemyAbilities1.activeEnemies.Remove(this);
            Debug.Log("Przeciwnik zosta³ pokonany!");
            Destroy(gameObject);
            pause.WinScreenPause();
        }
    }


    public void AttackPlayer()
    {
        Vector3Int playerHexPos = hexTilemap.WorldToCell(player.transform.position);
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(transform.position);

        int distance = HexDistance(enemyHexPos, playerHexPos);

        if (distance <= 1)
        {
            int damage = 20;
            var playerCharacterData = player.GetComponent<PlayerAbilities1>().characterData;

            playerAbilities.TakeDamage(damage);
            Debug.Log($"Przeciwnik zadaje graczowi {damage} obra¿eñ! Dystans: {distance}");
        }

    }

    private int HexDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.y - b.y));
    }

}
