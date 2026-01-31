using System.Numerics;
using System.Security;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData", menuName = "Scriptable Objects/TileData")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;
    public string tileName;


    public void FireTile(GameObject go)
    {
        UnityEngine.Debug.Log("LOL Get REKT");
        Destroy(go);
    }
    public void WaterTile(GameObject go)
    {
        if (go.CompareTag("Player"))
        {
        }
    }
    public void EarthTile(GameObject go)
    {
        UnityEngine.Debug.Log("this is a Earth tile");
    }

}
