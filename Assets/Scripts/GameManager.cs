using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    public Animator animator;
    public TMP_Text turnText;
    public static GameManager instance;
    public GameState currentGameState;
    public GameObject pauseMenuUI;
    public bool GameIsPaused = false;
    string oldSceneName;
    public GameObject winScreen;
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
    void Start()
    {
        //SoundEffectManager.Play("MenuMusic");
        map = GameObject.FindWithTag("Tileamap").GetComponent<Tilemap>();
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

                turnText.text = "Monster Turn";
                EnemyTurn();
                yield return new WaitForSeconds(0.5f);
                turnText.text = "Player Turn";
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
    /// UI Stuff
    /// </summary>
    /// 

    IEnumerator LoadLevelName(string levelName)
    {
        //oldSceneName = SceneManager.GetActiveScene().name;
        if (animator != null)
        {
            animator.SetTrigger("Start");
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene(levelName);
        animator.SetTrigger("End");
    }
    IEnumerator LoadLevelBuildIndex()
    {
        //oldSceneName = SceneManager.GetActiveScene().name;
        if (animator != null)
        {
            animator.SetTrigger("Start");
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        animator.SetTrigger("End");
    }


    public void WinScreen()
    {
        winScreen.SetActive(true);
    }
    public void NextPuzzle()
    {
        winScreen.SetActive(false);
        StartCoroutine(LoadLevelBuildIndex());
    }
    public void LoadMainMenu()
    {
        winScreen.SetActive(false);
        oldSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadLevelName("MainMenu"));
    }
    public void LoadLastScene()
    {
        StartCoroutine(LoadLevelName(oldSceneName));
    }
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
