using System.Diagnostics;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData", menuName = "Scriptable Objects/TileData")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;
    public string tileName;


    public void FireTile(GameObject go)
    {
        UnityEngine.Debug.Log("REKT");
        if (go.CompareTag("Player"))
        {
            UnityEngine.Debug.Log("GameOver");
        }
        Destroy(go);
    }
    public void WaterTile(GameObject go)
    {
        if (go.CompareTag("Player"))
        {
            Player player = go.GetComponent<Player>();
            player.isMoving = true;
            player.targetPosition = go.transform.position + new Vector3(0f, -1.0f, 0f);
        }
        else if (go.CompareTag("Enemy"))
        {
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.isMoving = true;
            enemy.targetPosition = go.transform.position + new Vector3(0f, -1.0f, 0f);
        }
    }
    public void EarthTile(GameObject go)
    {
        if(GameManager.instance.currentGameState == GameManager.GameState.playerTurn)
        {
            GameManager.instance.ChangeGameState(GameManager.GameState.enemyTurn);
        }
        else if(GameManager.instance.currentGameState == GameManager.GameState.enemyTurn)
        {
            GameManager.instance.ChangeGameState(GameManager.GameState.playerTurn);
        }
    }

}
