using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HealthBarFollow : MonoBehaviour
{
    public Transform target;  // Gracz lub Przeciwnik
    public Camera mainCamera;     // Kamera 2D (Main Camera)
    public Vector3 offset = new Vector3(0, 1, 0); // Opcjonalny offset, aby pasek zdrowia by� wy�ej nad postaci�

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;  // Automatycznie przypisz g��wn� kamer�, je�li nie zosta�a przypisana w inspektorze
        }

        if (target == null)
        {
            Debug.LogWarning("HealthBarFollow: Brak przypisanego targetu! Upewnij si�, �e target zosta� przypisany w inspektorze.");
        }
    }

    void Update()
    {
        if (target != null && mainCamera != null)
        {
            // Przekszta�camy pozycj� 2D targetu na wsp�rz�dne ekranu
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position + offset);

            // Ustawiamy pozycj� UI paska zdrowia na ekranie
            transform.position = screenPosition;
        }
    }
}


