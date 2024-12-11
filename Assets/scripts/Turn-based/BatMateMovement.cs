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
        if (Input.GetMouseButtonDown(0) && !hasMoved && isActive) // Gracz mo¿e klikn¹æ tylko raz na turê
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

            // Tworzymy obiekty Hex z pozycji bie¿¹cej i docelowej
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
                    Debug.Log("Nie mo¿na poruszyæ siê na kafelek zajêty przez przeciwnika.");
                }
            }
            else
            {
                Debug.Log("Pozycja jest poza zasiêgiem ruchu.");
            }
        }
        else
        {
            Debug.LogWarning("Klikniêto poza heksagonaln¹ siatk¹.");
        }
    }


    bool IsPositionOccupiedByEnemy(Vector3 position)
    {
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(enemy.transform.position);
        Vector3Int targetHexPos = hexTilemap.WorldToCell(position);
        return enemyHexPos == targetHexPos;  // Zwraca true, jeœli pozycja jest zajêta przez przeciwnika
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


