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
    public float timeBetweenTurns = 0.25f;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
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
                yield return new WaitForSeconds(timeBetweenTurns);

                turnText.text = "Monster Turn";
                EnemyTurn();
                yield return new WaitForSeconds(timeBetweenTurns);
                turnText.text = "Player Turn";
                ChangeGameState(GameState.playerTurn);
                break;
        }
    }

    public enum GameState
    {
        playerTurn = 0,
        enemyTurn = 1
    }
    public TileData GetTile(Vector3 entered)
    {
        return MapManager.instance.GetTileFromMap(entered);
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
            animator.SetTrigger("End");
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene(levelName);
        animator.SetTrigger("Start");
    }
    IEnumerator LoadLevelBuildIndex()
    {
        //oldSceneName = SceneManager.GetActiveScene().name;
        if (animator != null)
        {
            animator.SetTrigger("End");
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        animator.SetTrigger("Start");
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
