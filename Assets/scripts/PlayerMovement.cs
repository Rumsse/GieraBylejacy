using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerMovement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public float moveSpeed = 5f;
    private Vector3 targetPosition;
    public bool hasMoved = false;  // Flaga okreœlaj¹ca, czy gracz zakoñczy³ ruch
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
        if (Input.GetMouseButtonDown(0) && !hasMoved)  // Gracz mo¿e klikn¹æ tylko raz na turê
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
            targetPosition.y += yOffset;  // Dodaj przesuniêcie w osi y
            hasMoved = true;
        }
        else
        {
            Debug.LogWarning("Klikniêto poza heksagonaln¹ siatk¹.");
        }
    }

    void MovePlayerToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition && hasMoved)
        {
            Debug.Log("Gracz zakoñczy³ ruch");
        }
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }
}


