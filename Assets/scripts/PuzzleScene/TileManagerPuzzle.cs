using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManagerPuzzle : MonoBehaviour
{
    public Tilemap hexTilemap;

    private Dictionary<Vector3Int, GameObject> occupiedTiles = new Dictionary<Vector3Int, GameObject>();


    public bool IsTileOccupied(Vector3Int tilePosition)
    {
        return occupiedTiles.ContainsKey(tilePosition) && occupiedTiles[tilePosition] != null;
    }


    public void OccupyTile(Vector3Int tilePosition, GameObject occupier)
    {
        if (!occupiedTiles.ContainsKey(tilePosition))
        {
            occupiedTiles[tilePosition] = occupier;

        }
        else
        {
            Debug.LogWarning($"Tile at {tilePosition} is already occupied!");

        }
    }


    public void ReleaseTile(Vector3Int tilePosition)
    {
        if (occupiedTiles.ContainsKey(tilePosition))
        {
            Debug.Log($"Releasing tile at {tilePosition}, previously occupied by {occupiedTiles[tilePosition].name}.");
            occupiedTiles.Remove(tilePosition);
        }
        else
        {
            Debug.LogWarning($"Attempted to release tile at {tilePosition}, but it is not occupied!");
        }
    }

    public bool MoveOccupier(Vector3Int fromPosition, Vector3Int toPosition)
    {
        if (IsTileOccupied(fromPosition) && !IsTileOccupied(toPosition))
        {
            GameObject occupier = occupiedTiles[fromPosition];
            ReleaseTile(fromPosition);
            OccupyTile(toPosition, occupier);
            return true;
        }
        else
        {
            Debug.LogWarning($"Cannot move from {fromPosition} to {toPosition}. Target tile might be occupied or source tile is empty.");
            return false;
        }
    }

    public Vector3Int GetTilePosition(Vector3 worldPosition)
    {
        return hexTilemap.WorldToCell(worldPosition);
    }

    public void UpdateTileOccupation(Vector3Int oldPosition, Vector3Int newPosition, GameObject occupier)
    {
        if (occupier == null)
        {
            Debug.LogWarning("Próba aktualizacji kafla z obiektem, który zosta³ zniszczony.");
            return;
        }

        Debug.Log($"UpdateTileOccupation: Occupier = {occupier.name}, Old Position = {oldPosition}, New Position = {newPosition}");
        ReleaseTile(oldPosition);
        OccupyTile(newPosition, occupier);
    }


    private void OnDrawGizmos()
    {
        if (occupiedTiles != null && hexTilemap != null)
        {
            Gizmos.color = Color.red;
            foreach (var tile in occupiedTiles.Keys)
            {
                Vector3 worldPosition = hexTilemap.CellToWorld(tile) + hexTilemap.tileAnchor;
                Gizmos.DrawWireCube(worldPosition, new Vector3(1, 1, 0));
            }
        }
    }

}
