using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager1 : MonoBehaviour
{
    public Tilemap hexTilemap;
    private HashSet<Vector3Int> occupiedTiles = new HashSet<Vector3Int>();

    public GameObject[] characters;



    private void Start()
    {
        InitializeTileOccupancy();
    }

    public void InitializeTileOccupancy()
    {
        occupiedTiles.Clear();

        foreach (var character in characters)
        {
            Vector3Int characterTilePosition = hexTilemap.WorldToCell(character.transform.position);
            occupiedTiles.Add(characterTilePosition);

        }
    }

    public void OccupyTile(Vector3Int tilePosition)
    {
        if (!occupiedTiles.Contains(tilePosition))
        {
            occupiedTiles.Add(tilePosition);
        }
    }

    public void FreeTile(Vector3Int tilePosition)
    {
        if (occupiedTiles.Contains(tilePosition))
        {
            occupiedTiles.Remove(tilePosition);
        }
    }

    public bool IsTileOccupied(Vector3Int tilePosition)
    {
        return occupiedTiles.Contains(tilePosition);
    }

    public void UpdateTileOccupation(Vector3Int currentHex, Vector3Int targetHex)
    {
        FreeTile(currentHex);
        OccupyTile(targetHex);
    }

}
