using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static TurnManager;


public class BatMateMovement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public Transform playerTransform;
    public EnemyMovement enemy;
    public TileManager tileManager;


    private int maxMoveDistance = 4;
    private float moveSpeed = 3f;
    private float yOffset = 0.3f;
    public bool hasMoved = true;
    public bool isActive = false;

    private Vector3 targetPosition;


    void Start()
    {
        ResetMovement();
        targetPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasMoved && isActive) // Gracz mo�e klikn�� tylko raz na tur�
        {
            HandleMouseClick();
        }

        MoveBatMateToTarget();

    }

    void HandleMouseClick()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector3Int hexPosition = hexTilemap.WorldToCell(mouseWorldPos);

        if (hexTilemap.HasTile(hexPosition))
        {
            Vector3 targetWorldPosition = hexTilemap.CellToWorld(hexPosition) + hexTilemap.tileAnchor + new Vector3(0, yOffset, 0);

            // Tworzymy obiekty Hex z pozycji bie��cej i docelowej
            Hex playerHex = new Hex(hexTilemap.WorldToCell(transform.position).x, hexTilemap.WorldToCell(transform.position).y);
            Hex targetHex = new Hex(hexPosition.x, hexPosition.y);

            if (playerHex.HexDistance(targetHex) <= maxMoveDistance)
            {
                if (!tileManager.IsTileOccupied(hexPosition))
                {
                    Vector3Int currentHexPosition = hexTilemap.WorldToCell(transform.position);
                    //tileManager.UpdateTileOccupation(currentHexPosition, hexPosition);

                    targetPosition = targetWorldPosition;
                    hasMoved = true;
                }
                else
                {
                    Debug.Log("Nie mo�na poruszy� si� na zaj�ty kafelek.");
                }
            }
            else
            {
                Debug.Log("Pozycja jest poza zasi�giem ruchu.");
            }
        }
        else
        {
            Debug.LogWarning("Klikni�to poza heksagonaln� siatk�.");
        }
    }

    void MoveBatMateToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition && hasMoved) 
        {

        }
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }


}


