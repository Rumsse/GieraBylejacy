using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBarFollow : MonoBehaviour
{
    public Transform target;  // Gracz lub Przeciwnik
    public Camera mainCamera;     // Kamera 2D (Main Camera)
    public Vector3 offset = new Vector3(0, 1, 0); // Opcjonalny offset, aby pasek zdrowia by³ wy¿ej nad postaci¹

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;  // Automatycznie przypisz g³ówn¹ kamerê, jeœli nie zosta³a przypisana w inspektorze
        }

        if (target == null)
        {
            Debug.LogWarning("HealthBarFollow: Brak przypisanego targetu! Upewnij siê, ¿e target zosta³ przypisany w inspektorze.");
        }
    }

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
}


