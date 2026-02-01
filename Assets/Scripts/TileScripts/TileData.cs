using System.Diagnostics;
using System.Security;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData", menuName = "Scriptable Objects/TileData")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;
    public string tileName;



    public void GoalTile(GameObject go)
    {
        GameManager.instance.WinScreen();
        Player.instance.gameOver = true;
    }


    public void FireTile(GameObject go)
    {
        if (go.CompareTag("Player"))
        {
            if (Player.instance.currentPlayerState == Player.PlayerState.FireState)
            {
                Player.instance.animator.SetBool("isMoving", true);
                Player.instance.targetPosition = go.transform.position + new Vector3(-2.0f, 0.0f, 0f);
            }
            else if (Player.instance.currentPlayerState == Player.PlayerState.WaterState)
            {
                return;
            }
            else if (Player.instance.currentPlayerState == Player.PlayerState.EarthState)
            {
                Destroy(go);
            }
            else
            {
                Player.instance.stuck = true;
            }
            SoundEffectManager.Play("FireWhoosh");
            
        }
    }
    public void WaterTile(GameObject go)
    {
        if (go.CompareTag("Player"))
        {
            if (Player.instance.currentPlayerState == Player.PlayerState.FireState)
            {
                Destroy(go);
            }
            else if (Player.instance.currentPlayerState == Player.PlayerState.WaterState)
            {
                Player.instance.animator.SetBool("isMoving", true);
                Player.instance.targetPosition = go.transform.position + new Vector3(0f, -1.0f, 0f);

            }
            else if (Player.instance.currentPlayerState == Player.PlayerState.EarthState)
            {
                return;
            }
            else
            {
                Player.instance.stuck = true;
            }
            SoundEffectManager.Play("WaterSplash");
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
        if (go.CompareTag("Player"))
        {
            if (Player.instance.currentPlayerState == Player.PlayerState.FireState)
            {
                return;
            }
            else if (Player.instance.currentPlayerState == Player.PlayerState.WaterState)
            {
                Destroy(go);

            }
            else if (Player.instance.currentPlayerState == Player.PlayerState.EarthState)
            {
                Player.instance.animator.SetBool("isMoving", true);
                Player.instance.targetPosition = go.transform.position + new Vector3(0f, 1.0f, 0f);
            }
            else
            {
                Player.instance.stuck = true;
            }
            SoundEffectManager.Play("PlantGrowth");
            
        }
        else if (go.CompareTag("Enemy"))
        {
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.stuck = true;
        }
    }

}
