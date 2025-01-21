using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAbilities : MonoBehaviour
{
    public GameObject player; 
    private PlayerAbilitiesBoss playerAbilities;

    public int TurnsBetweenDebuff = 3;
    private int currentTurnCooldown = 0;

    public bool isAlive = true;



    public void PerformDebuff()
    {
        StatusEffect debuff = new StatusEffect("Os³abienie", 3, 0.5f, -1); 
        playerAbilities.ApplyStatus(debuff);
        Debug.Log("Boss rzuci³ os³abienie na gracza");
    }


}
