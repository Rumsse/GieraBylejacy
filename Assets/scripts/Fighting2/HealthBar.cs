using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Vector3 offset = new Vector3(0, 1, 0);
    public CharactersData characterData;


    void Start()
    {
        SetMaxHealth(characterData.maxHealth);
        SetHealth(characterData.health);

        characterData.OnHealthChanged += UpdateHealthUI;
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void UpdateHealthUI()
    {
        SetHealth(characterData.health);
    }

    //jak coœ nie dzia³a upewniæ siê ¿e w healthbar (child of character), gdzie jest przypisany ten skrypt jest przypisany scriptable object do CharactersData

}
