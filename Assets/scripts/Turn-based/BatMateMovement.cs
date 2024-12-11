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

    public float moveSpeed = 4f;
    public float yOffset = 0.3f;
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

            int maxMoveDistance = 4; 
            if (playerHex.HexDistance(targetHex) <= maxMoveDistance)
            {
                if (!IsPositionOccupiedByEnemy(targetWorldPosition))
                {
                    targetPosition = targetWorldPosition;
                    hasMoved = true;
                }
                else
                {
                    Debug.Log("Nie mo�na poruszy� si� na kafelek zaj�ty przez przeciwnika.");
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


    bool IsPositionOccupiedByEnemy(Vector3 position)
    {
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(enemy.transform.position);
        Vector3Int targetHexPos = hexTilemap.WorldToCell(position);
        return enemyHexPos == targetHexPos;  // Zwraca true, je�li pozycja jest zaj�ta przez przeciwnika
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


