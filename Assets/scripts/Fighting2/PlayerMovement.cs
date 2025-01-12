using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerMovement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public Transform playerTransform;
    public EnemyMovement enemy;
    public PlayerAbilities playerAbilities;
    public TileManager tileManager;

    public float moveSpeed = 4f;
    public int maxMoveDistance = 4;
    public float yOffset = 0.3f;
    public bool hasMoved = false;
    private bool hasLogged = false;
    public bool isActive = true;

    private Vector3 targetPosition;



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
        if (Input.GetMouseButtonDown(0) && !hasMoved && (!playerAbilities.isAttackMode1 && !playerAbilities.isAttackMode2)) // Gracz mo�e klikn�� tylko raz na tur�
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
            Vector3 targetWorldPosition = hexTilemap.CellToWorld(hexPosition) + hexTilemap.tileAnchor + new Vector3(0, yOffset, 0);

            // Tworzymy obiekty Hex z pozycji bie��cej i docelowej
            Hex playerHex = new Hex(hexTilemap.WorldToCell(transform.position).x, hexTilemap.WorldToCell(transform.position).y);
            Hex targetHex = new Hex(hexPosition.x, hexPosition.y);
;
            if (playerHex.HexDistance(targetHex) <= maxMoveDistance)
            {
                if (!tileManager.IsTileOccupied(hexPosition)) 
                {
                    Vector3Int currentHexPosition = hexTilemap.WorldToCell(transform.position);
                    tileManager.UpdateTileOccupation(currentHexPosition, hexPosition);

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


    void MovePlayerToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition && hasMoved) //chyba jak si� doda "&& !isMoving" czy co� na podobnej zasadzie to mo�e debug przestanie si� wy�wietla� co milisekunde
        {
            //Debug.Log("Gracz zako�czy� ruch"); (denerwuje mnie ten debug)
            hasLogged = true; //przetestowa� czy to si� op�aca zostawi�, bo narazie nie dzia�a
        }
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }


}


