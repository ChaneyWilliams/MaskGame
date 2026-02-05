using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    public Tilemap map;
    [SerializeField] private List<TileData> tileDatas;
    public List<TileBase> allTiles;

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



    // -------------------------------
    // Tile lookup helpers
    // -------------------------------
    public TileBase GetTileBase(int tileIndex)
    {
        return allTiles[tileIndex];
    }

    public TileData GetTileFromMap(Vector3 position)
    {
        Vector3Int gridPosition = map.WorldToCell(position);

        TileBase tile = map.GetTile(gridPosition);

        if (tile == null) return null;
        return dataFromTile[tile];
    }

    public void TileChoices(TileData tileInfo, GameObject entered)
    {
        switch (tileInfo.tileState)
        {
            case TileData.TileState.FireTile:
                tileInfo.FireTile(entered);
                break;

            case TileData.TileState.EarthTile:
                tileInfo.EarthTile(entered);
                break;

            case TileData.TileState.WaterTile:
                tileInfo.WaterTile(entered);
                break;

            case TileData.TileState.GoalTile:
                tileInfo.GoalTile(entered);
                break;

            case TileData.TileState.NormalTile:
            default:
                break;
        }
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
            TileData tile = GetTileFromMap(pos);
            if (tile == null || tile.tileState == TileData.TileState.NormalTile || tile.tileState == TileData.TileState.WallTile)
                continue;

            specialTiles.Add(pos);
        }

        Dictionary<Vector3Int, TileBase> allChanges = new Dictionary<Vector3Int, TileBase>();

        foreach (Vector3Int tile in specialTiles)
        {

            var changes = TileCheck(tile);

            foreach (var kvp in changes)
            {
                allChanges[kvp.Key] = kvp.Value;
            }
        }

        foreach (var kvp in allChanges)
        {
            map.SetTile(kvp.Key, kvp.Value);
        }
    }

    Dictionary<Vector3Int, TileBase> TileCheck(Vector3Int position)
    {

        Dictionary<Vector3Int, TileBase> changes = new Dictionary<Vector3Int, TileBase>();
        TileData currentTile = GetTileFromMap(position);
        if (currentTile == null)
            return changes;

        foreach (Vector3Int nextPos in GetNeighbors(position))
        {
            TileData neighborTile = GetTileFromMap(nextPos);
            if (neighborTile == null)
                continue;
            switch ((currentTile.tileState, neighborTile.tileState))
            {
                case (TileData.TileState.EarthTile, TileData.TileState.WaterTile):
                    changes[nextPos] = allTiles[0];
                    break;

                case (TileData.TileState.FireTile, TileData.TileState.EarthTile):
                    changes[nextPos] = allTiles[1];
                    break;

                case (TileData.TileState.WaterTile, TileData.TileState.FireTile):
                    changes[nextPos] = allTiles[2];
                    break;
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
