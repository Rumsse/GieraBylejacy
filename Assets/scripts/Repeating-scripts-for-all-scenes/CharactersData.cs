using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharactersData", menuName = "Game/CharactersData")]
public class CharactersData : ScriptableObject
{
    public string characterName;
    public int maxHealth;
    public int health;
    public bool isAlive = true;

    public event System.Action OnHealthChanged;


    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            isAlive = false;
        }

        OnHealthChanged?.Invoke();

    }

    public void Heal(int amount)
    {
        if (isAlive)
        {
            health += amount;

            if (health > maxHealth)
            {
                health = maxHealth;
            }

            OnHealthChanged?.Invoke();
        }
    }

    public void ResetData()
    {
        health = maxHealth;
        isAlive = true;
    }



}
