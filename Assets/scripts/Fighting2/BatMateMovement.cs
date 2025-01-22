using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using static TurnManager;


public class BatMateMovement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public TileManager tileManager;

    Animator animator;

    private int maxMoveDistance = 4;
    private float moveSpeed = 3f;
    public float yOffset = 0.5f;

    public bool hasMoved = true;
    public bool isActive = false;

    private Vector3 targetPosition;
    private Vector3Int currentHexPosition;
    private Vector3 occupiedTileOffset = new Vector3(0, -0.5f, 0);


    void Start()
    {
        targetPosition = transform.position + new Vector3(0, yOffset, 0);
        transform.position = targetPosition;

        animator = GetComponent<Animator>();

        Vector3Int startHex = tileManager.GetTilePosition(transform.position);
        currentHexPosition = tileManager.GetTilePosition(transform.position);
        tileManager.OccupyTile(startHex, gameObject);
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
            Vector3Int currentHexPosition = tileManager.GetTilePosition(transform.position);

            // Tworzymy obiekty Hex z pozycji bie¿¹cej i docelowej
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
                    Debug.Log("Nie mo¿na poruszyæ siê na zajêty kafelek.");
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

    IEnumerator MoveBatMateToTarget()
    {
        //animator.SetBool("isWalking", true);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        Vector3Int newHexPosition = tileManager.GetTilePosition(transform.position - new Vector3(0, yOffset, 0));

        tileManager.UpdateTileOccupation(currentHexPosition, newHexPosition, gameObject);
        currentHexPosition = newHexPosition;

        //animator.SetBool("isWalking", false);
        hasMoved = true;
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }


}


