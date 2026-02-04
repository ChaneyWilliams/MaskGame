using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    [SerializeField] private Tilemap map;
    [SerializeField] private List<TileData> tileDatas;
    [SerializeField] private List<TileBase> allTiles;

    private Dictionary<TileBase, TileData> dataFromTile;
    private List<Vector3Int> specialTiles = new List<Vector3Int>();

    private readonly List<Vector3Int> directions = new List<Vector3Int>
    {
        Vector3Int.left,
        Vector3Int.down,
        Vector3Int.right,
        Vector3Int.up,
    };

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // Prefer assigning via Inspector
        if (map == null)
            map = GameObject.FindWithTag("Tilemap")?.GetComponent<Tilemap>();

        dataFromTile = new Dictionary<TileBase, TileData>();

        foreach (TileData tileData in tileDatas)
        {
            foreach (TileBase tile in tileData.tiles)
            {
                if (!dataFromTile.ContainsKey(tile))
                    dataFromTile.Add(tile, tileData);
            }
        }
    }

    public TileData GetTileFromMap(Vector3 position)
    {
        Vector3Int gridPosition = map.WorldToCell(position);

        TileBase tile = map.GetTile(gridPosition);

        if (tile == null) return null;
        return dataFromTile[tile];
        //Debug.Log(tileInfo.tileName);
    }

    // -------------------------------
    // Tile lookup helpers
    // -------------------------------

    public TileData GetTileFromCell(Vector3 position)
    {
        Vector3Int gridPosition = map.WorldToCell(position);

        TileBase tile = map.GetTile(gridPosition);

        if (tile == null) return null;
        return dataFromTile[tile];
        //Debug.Log(tileInfo.tileName);
    }

    // -------------------------------
    // Turn logic
    // -------------------------------
    public void TakeTurn()
    {
        specialTiles.Clear();

        BoundsInt bounds = map.cellBounds;

        foreach (Vector3Int pos in bounds.allPositionsWithin)
        {
            TileData tile = GetTileFromCell(pos);
            if (tile == null || tile.tileName == "Normal")
                continue;

            specialTiles.Add(pos);
        }

        Dictionary<Vector3Int, TileBase> allChanges = new Dictionary<Vector3Int, TileBase>();

        foreach (Vector3Int tile in specialTiles)
        {
            var changes = TileCheck(tile);
            foreach (var kvp in changes)
            {
                allChanges[kvp.Key] = kvp.Value; // Merge all changes
            }
        }

        // Apply all changes at once
        foreach (var kvp in allChanges)
        {
            map.SetTile(kvp.Key, kvp.Value);
        }
    }

    Dictionary<Vector3Int, TileBase> TileCheck(Vector3Int position)
    {
        Dictionary<Vector3Int, TileBase> changes = new Dictionary<Vector3Int, TileBase>();
        TileData currentTile = GetTileFromCell(position);
        if (currentTile == null)
            return changes;

        foreach (Vector3Int nextPos in GetNeighbors(position))
        {
            TileData neighborTile = GetTileFromCell(nextPos);
            if (neighborTile == null)
                continue;

            if (currentTile.tileName == "Earth" && neighborTile.tileName == "Water")
            {
                changes[nextPos] = allTiles[0]; // Water -> something
            }
            else if (currentTile.tileName == "Fire" && neighborTile.tileName == "Earth")
            {
                changes[nextPos] = allTiles[1]; // Earth -> something
            }
            else if (currentTile.tileName == "Water" && neighborTile.tileName == "Fire")
            {
                changes[nextPos] = allTiles[2]; // Fire -> something
            }
        }

        return changes;
    }



    List<Vector3Int> GetNeighbors(Vector3Int start)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        foreach (Vector3Int direction in directions)
        {
            neighbors.Add(start + direction);
        }
        return neighbors;
    }
}
