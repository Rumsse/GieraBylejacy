using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Transform target;
    public Camera mainCamera;
    public Vector3 offset = new Vector3(0, 1, 0);


    void Update()
    {
        if (target != null && mainCamera != null)
        {
            // Przekszta³camy pozycjê 2D targetu na wspó³rzêdne ekranu
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position + offset);

            // Ustawiamy pozycjê UI paska zdrowia na ekranie
            transform.position = screenPosition;
        }
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

}
