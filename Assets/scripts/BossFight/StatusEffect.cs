using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    public string EffectName; 
    public int Duration; 
    public float DamageModifier; // Modyfikator obra¿eñ (np. 0.5 dla 50%)
    public int MovementReduction; // Modyfikator ruchu (np. -1)

    public StatusEffect(string name, int duration, float damageMod, int movementRed)
    {
        EffectName = name;
        Duration = duration;
        DamageModifier = damageMod;
        MovementReduction = movementRed;
    }
}

