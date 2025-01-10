using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemyMovement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public Transform playerTransform;
    public Transform enemyTransform;

    public float moveSpeed = 2f;
    public float yOffset = 0.3f;
    public int maxMoveDistance = 3;
    public bool hasMoved = false;
    private bool isMoving = false;
    public bool isActive = true;

    private Vector3 targetPosition;


    void Start()
    {
        if (hexTilemap == null)
        {
            hexTilemap = GameObject.Find("HexTilemap").GetComponent<Tilemap>();
        }

        if (playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        targetPosition = transform.position;

    }

    void Update()
    {
        if (isMoving)
        {
            MoveEnemyToTarget();
        }
    }

    public void MoveTowardsPlayer()
    {
        Vector3Int playerHexPos = hexTilemap.WorldToCell(playerTransform.position); //pozycja hexa na którym znajduje siê gracz
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(transform.position);
        Vector3Int nextHexPos = GetNextStepTowards(playerHexPos, enemyHexPos); //oblicza kolejny krok w stronê gracza

        Hex enemyHex = new Hex(enemyHexPos.x, enemyHexPos.y); // Tworzymy obiekty Hex z pozycji przeciwnika i gracza
        Hex playerHex = new Hex(playerHexPos.x, playerHexPos.y); // Tworzymy obiekty Hex z pozycji przeciwnika i gracza

        if (hexTilemap.HasTile(nextHexPos))
        {
            Vector3 targetWorldPosition = hexTilemap.CellToWorld(nextHexPos) + hexTilemap.tileAnchor + new Vector3(0, yOffset, 0);

            if (!IsPositionOccupiedByPlayer(targetWorldPosition))
            {
                targetPosition = targetWorldPosition;
                isMoving = true;
                hasMoved = true;
                Debug.Log("Przeciwnik zmierza do pozycji: " + targetPosition);
            }
            else
            {
                Debug.Log("Nie mo¿na poruszyæ siê na kafelek zajêty przez gracza.");
            }
        }
        else
        {
            Debug.Log("Docelowy kafelek nie istnieje.");
        }

        if (!hexTilemap.HasTile(nextHexPos))
        {
            Debug.Log("Docelowy kafelek nie istnieje. Szukanie alternatywnej drogi...");
        }

    }


    bool IsPositionOccupiedByPlayer(Vector3 position)
    {
        Vector3Int playerHexPos = hexTilemap.WorldToCell(playerTransform.position);
        Vector3Int targetHexPos = hexTilemap.WorldToCell(position);
        return playerHexPos == targetHexPos;  // Zwraca true, jeœli pozycja jest zajêta przez gracza
    }

    public void MoveEnemyToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isMoving = false;
            hasMoved = true;
            Debug.Log("Przeciwnik dotar³ do docelowej pozycji.");
        }
    }

    Vector3Int GetNextStepTowards(Vector3Int target, Vector3Int start)
    {
        int dq = target.x - start.x;
        int dr = target.y - start.y;

        if (Mathf.Abs(dq) > Mathf.Abs(dr))
        {
            start.x += (dq > 0) ? 1 : -1;
        }
        else
        {
            start.y += (dr > 0) ? 1 : -1;
        }

        return start;
    }

    public void ResetMovement()
    {
        hasMoved = false;
    }
}
