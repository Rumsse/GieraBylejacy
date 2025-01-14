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
    public Animator animator;

    public float moveSpeed = 5f;
    public float yOffset = 0.3f;
    public bool hasMoved = false;
    private bool hasLogged = false;

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
        if (Input.GetMouseButtonDown(0) && !hasMoved && (!playerAbilities.isAttackMode1 || !playerAbilities.isAttackMode2)) // Gracz mo�e klikn�� tylko raz na tur�
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

            // Sprawd� odleg�o�� w heksach
            int maxMoveDistance = 4; // Maksymalny zasi�g ruchu
            if (playerHex.HexDistance(targetHex) <= maxMoveDistance)
            {
                if (!IsPositionOccupiedByEnemy(targetWorldPosition))  // Sprawd�, czy pozycja nie jest zaj�ta
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

    void MovePlayerToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
        if (transform.position != targetPosition)
        {
            animator.SetFloat("Speed", 1f);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }


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
