using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemyMovement : MonoBehaviour
{
    public Tilemap hexTilemap;
    public Transform playerTransform;
    public float moveSpeed = 2f;
    private Vector3 targetPosition;
    public bool hasMoved = false;  // Flaga dla tury przeciwnika
    private bool isMoving = false;
    public float yOffset = 1.2f;

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
        Vector3Int playerHexPos = hexTilemap.WorldToCell(playerTransform.position);
        Vector3Int enemyHexPos = hexTilemap.WorldToCell(transform.position);

        Vector3Int nextHexPos = GetNextStepTowards(playerHexPos, enemyHexPos);

        targetPosition = hexTilemap.CellToWorld(nextHexPos) + hexTilemap.tileAnchor;
        targetPosition.y += yOffset;  // Dodaj przesuniêcie w osi y

        isMoving = true;
    }

    void MoveEnemyToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            isMoving = false;
            hasMoved = true;  // Ustaw flagê po zakoñczeniu ruchu
        }
    }

    public void ResetMovement()
    {
        hasMoved = false;
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
}


