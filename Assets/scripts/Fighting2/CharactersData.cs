using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharactersData", menuName = "Game/CharactersData")]
public class CharactersData : ScriptableObject
{
    public string characterName;
    public int health;
    public bool isAlive = true;



    public void Heal(int amount)
    {
        if (isAlive)
        {
            health += amount;
        }
    }

}
