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
        if (Input.GetMouseButtonDown(0) && !hasMoved && (!playerAbilities.isAttackMode1 || !playerAbilities.isAttackMode2)) // Gracz mo¿e klikn¹æ tylko raz na turê
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

            // Tworzymy obiekty Hex z pozycji bie¿¹cej i docelowej
            Hex playerHex = new Hex(hexTilemap.WorldToCell(transform.position).x, hexTilemap.WorldToCell(transform.position).y);
            Hex targetHex = new Hex(hexPosition.x, hexPosition.y);

            // SprawdŸ odleg³oœæ w heksach
            int maxMoveDistance = 4; // Maksymalny zasiêg ruchu
            if (playerHex.HexDistance(targetHex) <= maxMoveDistance)
            {
                if (!IsPositionOccupiedByEnemy(targetWorldPosition))  // SprawdŸ, czy pozycja nie jest zajêta
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

    void MovePlayerToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition && hasMoved) //chyba jak siê doda "&& !isMoving" czy coœ na podobnej zasadzie to mo¿e debug przestanie siê wyœwietlaæ co milisekunde
        {
            //Debug.Log("Gracz zakoñczy³ ruch"); (denerwuje mnie ten debug)
            hasLogged = true; //przetestowaæ czy to siê op³aca zostawiæ, bo narazie nie dzia³a
        }
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }


}
