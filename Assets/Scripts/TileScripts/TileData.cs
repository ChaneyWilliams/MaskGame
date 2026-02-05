using System.Diagnostics;
using System.Security;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "TileData", menuName = "Scriptable Objects/TileData")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;
    public TileState tileState;



    public void GoalTile(GameObject go)
    {
        if (SceneManager.GetActiveScene().name == "Puzzle5")
        {
            GameManager.instance.Congrats();
        }
        else
        {
            GameManager.instance.WinScreen();
            Player.instance.gameOver = true;
        }
    }


    public void FireTile(GameObject go)
    {
        if (!go.CompareTag("Player"))
            return;

        switch (Player.instance.currentPlayerState)
        {
            case Player.PlayerState.FireState:
                Player.instance.animator.SetBool("isMoving", true);
                Player.instance.moveTargetPos = go.transform.position + new Vector3(-2.0f, 0.0f, 0f);
                break;

            case Player.PlayerState.WaterState:
                SoundEffectManager.Play("FireWhoosh");
                return;

            case Player.PlayerState.EarthState:
                Destroy(go);
                break;

            default:
                Player.instance.stuck = true;
                break;
        }

        SoundEffectManager.Play("FireWhoosh");
    }

    public void WaterTile(GameObject go)
    {
        if (go.CompareTag("Player"))
        {
            switch (Player.instance.currentPlayerState)
            {
                case Player.PlayerState.FireState:
                    Destroy(go);
                    break;

                case Player.PlayerState.WaterState:
                    Player.instance.animator.SetBool("isMoving", true);
                    Player.instance.moveTargetPos = go.transform.position + new Vector3(0f, -1.0f, 0f);
                    break;

                case Player.PlayerState.EarthState:
                    SoundEffectManager.Play("WaterSplash");
                    return;

                default:
                    Player.instance.stuck = true;
                    break;
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
            switch (Player.instance.currentPlayerState)
            {
                case Player.PlayerState.FireState:
                    SoundEffectManager.Play("PlantGrowth");
                    return;

                case Player.PlayerState.WaterState:
                    Destroy(go);
                    break;

                case Player.PlayerState.EarthState:
                    Player.instance.animator.SetBool("isMoving", true);
                    Player.instance.moveTargetPos = go.transform.position + new Vector3(0f, 1.0f, 0f);
                    break;

                default:
                    Player.instance.stuck = true;
                    break;
            }

            SoundEffectManager.Play("PlantGrowth");
        }
        else if (go.CompareTag("Enemy"))
        {
            Enemy enemy = go.GetComponent<Enemy>();
            enemy.stuck = true;
        }
    }


    public enum TileState
    {
        NormalTile = 0,
        EarthTile = 1,
        FireTile = 2,
        WaterTile = 3,
        GoalTile = 4,
        WallTile = 5

    }


}
