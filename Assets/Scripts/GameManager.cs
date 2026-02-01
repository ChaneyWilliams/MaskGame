using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentGameState;
    public GameObject pauseMenuUI;
    public bool GameIsPaused = false;
    [SerializeField] private Tilemap map;

    [SerializeField] List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTile;

    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }

        dataFromTile = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            foreach (var tile in tileData.tiles)
            {
                dataFromTile.Add(tile, tileData);
            }
        }
    }
public void ChangeGameState(GameState newGameState)
{
    StopAllCoroutines();
    StartCoroutine(ChangeGameStateRoutine(newGameState));
}

private IEnumerator ChangeGameStateRoutine(GameState newGameState)
{
    currentGameState = newGameState;
    //Debug.Log(currentGameState);

    switch (currentGameState)
    {
        case GameState.playerTurn:
            yield break;

        case GameState.enemyTurn:
            yield return new WaitForSeconds(0.5f);

            EnemyTurn();

            yield return new WaitForSeconds(0.5f);

            ChangeGameState(GameState.playerTurn);
            break;
    }
}


    public TileData GetTile(Vector3 entered)
    {
        Vector3Int gridPosition = map.WorldToCell(entered);

        TileBase tile = map.GetTile(gridPosition);

        if (tile == null) return null;

        return dataFromTile[tile];
        //Debug.Log(tileInfo.tileName);
    }

    public enum GameState
    {
        playerTurn = 0,
        enemyTurn = 1
    }

    void EnemyTurn()
    {
        EnemyManager.instance.TakeTurn();
    }



    /// <summary>
    /// PauseMenu Stuff
    /// </summary>
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        Time.timeScale = 1.0f;
    }
    public void Paused()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    public void Quite()
    {
        Application.Quit();
    }
}
