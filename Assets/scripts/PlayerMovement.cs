using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerMovement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    public bool hasMoved = false;  // Flaga okre�laj�ca, czy gracz zako�czy� ruch
    public float yOffset = 0.2f;

    void Start()
    {
        if (hexTilemap == null)
        {
            hexTilemap = GameObject.Find("HexTilemap").GetComponent<Tilemap>();
        }
        targetPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasMoved)  // Gracz mo�e klikn�� tylko raz na tur�
        {
            HandleMouseClick();
        }

        MovePlayerToTarget();
    }

    void HandleMouseClick()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3Int hexPosition = hexTilemap.WorldToCell(mouseWorldPos);

        if (hexTilemap.HasTile(hexPosition))
        {
            targetPosition = hexTilemap.CellToWorld(hexPosition) + hexTilemap.tileAnchor;
            targetPosition.y += yOffset;  // Dodaj przesuni�cie w osi y
            hasMoved = true;
        }
        else
        {
            Debug.LogWarning("Klikni�to poza heksagonaln� siatk�.");
        }
    }

    void MovePlayerToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition && hasMoved)
        {
            Debug.Log("Gracz zako�czy� ruch");
        }
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }
}


