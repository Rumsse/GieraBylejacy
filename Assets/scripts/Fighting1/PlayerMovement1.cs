using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerMovement1 : MonoBehaviour
{
    public Tilemap hexTilemap;
    public PlayerAbilities1 playerAbilities;
    public TileManager1 tileManager;

    Animator animator;

    private float moveSpeed = 3f;
    private int maxMoveDistance = 4;
    private float yOffset = 0.5f;

    public bool hasMoved = false;
    public bool isActive = true;

    private Vector3 targetPosition;
    private Vector3Int currentHexPosition;
    private Vector3 occupiedTileOffset = new Vector3(0, -0.5f, 0);



    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();

        Vector3Int startHex = tileManager.GetTilePosition(transform.position);
        currentHexPosition = tileManager.GetTilePosition(transform.position);
        tileManager.OccupyTile(startHex, gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasMoved && (!playerAbilities.isAttackMode1 && !playerAbilities.isAttackMode2)) // Gracz mo¿e klikn¹æ tylko raz na turê
        {
            HandleMouseClick();
        }

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
            ;
            if (Vector3.Distance(currentHexPosition, hexPosition) <= maxMoveDistance)
            {
                if (!tileManager.IsTileOccupied(hexPosition))
                {
                    currentHexPosition = hexPosition;

                    targetPosition = targetWorldPosition;
                    StartCoroutine(MovePlayerToTarget());
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


    IEnumerator MovePlayerToTarget()
    {
        animator.SetBool("isWalking", true);

        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        Vector3Int newHexPosition = tileManager.GetTilePosition(transform.position + occupiedTileOffset);

        tileManager.UpdateTileOccupation(currentHexPosition, newHexPosition, gameObject);
        currentHexPosition = newHexPosition;

        animator.SetBool("isWalking", false);
        hasMoved = true;
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }


}


