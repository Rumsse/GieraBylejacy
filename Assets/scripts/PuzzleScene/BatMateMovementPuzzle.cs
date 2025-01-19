using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static TurnManager;


public class BatMateMovementPuzzle : MonoBehaviour
{
    public Tilemap hexTilemap;
    public TileManagerPuzzle tileManager;

    private int maxMoveDistance = 4;
    private float moveSpeed = 3f;
    private float yOffset = 0.3f;

    public bool hasMoved = true;
    public bool isActive = false;
    public bool isBatMateMoving = false;
    public bool isAlive;

    private Vector3 targetPosition;
    private Vector3Int currentHexPosition;
    private Vector3 occupiedTileOffset = new Vector3(0, -0.5f, 0);


    void Start()
    {
        targetPosition = transform.position;
        Vector3Int startHex = tileManager.GetTilePosition(transform.position);
        currentHexPosition = tileManager.GetTilePosition(transform.position);
        tileManager.OccupyTile(startHex, gameObject);
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
            Vector3Int currentHexPosition = tileManager.GetTilePosition(transform.position);

            // Tworzymy obiekty Hex z pozycji bie��cej i docelowej
            Hex playerHex = new Hex(hexTilemap.WorldToCell(transform.position).x, hexTilemap.WorldToCell(transform.position).y);
            Hex targetHex = new Hex(hexPosition.x, hexPosition.y);

            if (Vector3.Distance(currentHexPosition, hexPosition) <= maxMoveDistance)
            {
                if (!tileManager.IsTileOccupied(hexPosition))
                {
                    currentHexPosition = hexPosition;

                    targetPosition = targetWorldPosition;
                    StartCoroutine(MoveBatMateToTarget());
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

    IEnumerator MoveBatMateToTarget()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        Vector3Int newHexPosition = tileManager.GetTilePosition(transform.position + occupiedTileOffset);

        tileManager.UpdateTileOccupation(currentHexPosition, newHexPosition, gameObject);
        currentHexPosition = newHexPosition;

        hasMoved = true;
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }


}
