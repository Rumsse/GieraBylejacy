using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPuzzle : MonoBehaviour
{
    public Slider slider;
    public Vector3 offset = new Vector3(0, 1, 0);


    public void SetMaxHealth(int currentHealth)
    {
        slider.maxValue = currentHealth;
        slider.value = currentHealth;
    }

    public void SetHealth(int currentHealth)
    {
        slider.value = currentHealth;
    }

}
