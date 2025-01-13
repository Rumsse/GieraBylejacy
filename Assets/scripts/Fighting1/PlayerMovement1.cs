using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerMovement1 : MonoBehaviour
{
    public Tilemap hexTilemap;
    public Transform playerTransform;
    public EnemyMovement1 enemy;
    public PlayerAbilities1 playerAbilities;
    public TurnManager1 turnManager;
    public TileManager1 tileManager;

    public float moveSpeed = 5f;
    public float yOffset = 0.3f;
    public bool hasMoved = false;
    public bool isPlayerMoving;

    public Animator animator;

    private Vector3 targetPosition;



    void Start()
    {
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !hasMoved && (!playerAbilities.isAttackMode1 || !playerAbilities.isAttackMode2)) // Gracz mo¿e klikn¹æ tylko raz na turê
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

            // Tworzymy obiekty Hex z pozycji bie¿¹cej i docelowej
            Hex playerHex = new Hex(hexTilemap.WorldToCell(transform.position).x, hexTilemap.WorldToCell(transform.position).y);
            Hex targetHex = new Hex(hexPosition.x, hexPosition.y);

            int maxMoveDistance = 4;
            if (playerHex.HexDistance(targetHex) <= maxMoveDistance)
            {
                if (!tileManager.IsTileOccupied(hexPosition))
                {
                    Vector3Int currentHexPosition = hexTilemap.WorldToCell(transform.position);
                    tileManager.UpdateTileOccupation(currentHexPosition, hexPosition);

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

        while (Vector3.Distance(transform.position, targetPosition) >0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            Debug.Log($"isWalking: {animator.GetBool("isWalking")}");

            yield return null;
        }
        
        transform.position = targetPosition; 
        animator.SetBool("isWalking", false);
        hasMoved = true;
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }


}
