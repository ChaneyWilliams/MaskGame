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
    int turnCounter = 1;
    public static GameManager instance;
    public GameState currentGameState;
    public GameObject pauseMenuUI;
    public bool GameIsPaused = false;
    public GameObject totalWinScreen;
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
            case GameState.PlayerTurn:
                yield break;

            case GameState.EnemyTurn:

                yield return new WaitForSeconds(timeBetweenTurns);
                turnText.text = "Monster Turn: " + turnCounter.ToString();

                EnemyTurn();

                yield return new WaitForSeconds(timeBetweenTurns);
                ChangeGameState(GameState.EnvironTurn);

                break;
            case GameState.EnvironTurn:

                yield return new WaitForSeconds(timeBetweenTurns);
                turnText.text = "Environment Turn: " + turnCounter.ToString();

                EnvironmentTurn();
                
                yield return new WaitForSeconds(timeBetweenTurns);
                turnCounter++;
                turnText.text = "Player Turn: " + turnCounter.ToString() ;
                ChangeGameState(GameState.PlayerTurn);

                break;
        }
    }

    public enum GameState
    {
        PlayerTurn = 0,
        EnemyTurn = 1,
        EnvironTurn = 2
    }
    public TileData GetTile(Vector3 entered)
    {
        return MapManager.instance.GetTileFromMap(entered);
    }

    void EnemyTurn()
    {
        EnemyManager.instance.TakeTurn();
    }
    void EnvironmentTurn()
    {
        MapManager.instance.TakeTurn();
    }



    /// <summary>
    /// UI Stuff
    /// </summary>
    /// 
    /// 
    public void Congrats()
    {
        totalWinScreen.SetActive(true);
    }

    IEnumerator LoadLevelName(string levelName)
    {
        //oldSceneName = SceneManager.GetActiveScene().name;
        if (animator != null)
        {
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene(levelName);
        animator.SetTrigger("FadeIn");
        turnCounter = 1;
    }
    IEnumerator LoadLevelBuildIndex()
    {
        //oldSceneName = SceneManager.GetActiveScene().name;
        if (animator != null)
        {
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        animator.SetTrigger("FadeIn");
        turnCounter = 1;
    }

    public void ResetLevel()
    {
        StartCoroutine(LoadLevelName(SceneManager.GetActiveScene().name));
    }


    public void WinScreen()
    {
        winScreen.SetActive(true);
    }
    public void NextPuzzle()
    {
        SoundEffectManager.Play("ButtonClick");
        winScreen.SetActive(false);
        StartCoroutine(LoadLevelBuildIndex());
    }
    public void LoadMainMenu()
    {
        SoundEffectManager.Play("ButtonClick");
        totalWinScreen.SetActive(false);
        winScreen.SetActive(false);
        oldSceneName = SceneManager.GetActiveScene().name;
        StartCoroutine(LoadLevelName("MainMenu"));
    }
    public void LoadLastScene()
    {
        SoundEffectManager.Play("ButtonClick");
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
        SoundEffectManager.Play("ButtonClick");
        Application.Quit();
    }
}
