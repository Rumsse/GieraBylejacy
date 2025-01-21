using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
    public string EffectName; 
    public int Duration; 
    public int BasicDamageModifier; 
    public int AdvancedDamageModifier; 
    public int MovementReduction; 

    public StatusEffect(string name, int duration, int damageMod1, int damageMod2, int movementRed)
    {
        EffectName = name;
        Duration = duration;
        BasicDamageModifier = damageMod1;
        AdvancedDamageModifier = damageMod2;
        MovementReduction = movementRed;
    }
}

