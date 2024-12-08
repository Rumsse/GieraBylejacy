using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
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

    //jak coœ nie bêdzie dzia³aæ poprawnie upewniæ siê, ¿e obiekt enemy ma tak¹ sam¹ wartoœæ maxHealth co maxvalue w sliderze w fill (healhbar)

}
