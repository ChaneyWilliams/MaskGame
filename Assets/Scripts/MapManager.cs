using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;
public class MapManager : MonoBehaviour
{

    public static MapManager instance;
    [SerializeField] private Tilemap map;

    [SerializeField] List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTile;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(gameObject); }
        map =  GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();

        dataFromTile = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTile.Add(tile, tileData);
            }
        }
    }


    public TileData GetTileFromMap(Vector3 entered)
    {
        Vector3Int gridPosition = map.WorldToCell(entered);

        TileBase tile = map.GetTile(gridPosition);

        if (tile == null) return null;

        return dataFromTile[tile];
        //Debug.Log(tileInfo.tileName);
    }
}
