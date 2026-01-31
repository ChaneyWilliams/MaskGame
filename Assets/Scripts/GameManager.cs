using System.Dynamic;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentGameState;
    public GameObject pauseMenuUI;
    public bool GameIsPaused = false;

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
        currentGameState = newGameState;
        Debug.Log(currentGameState);

        switch (currentGameState)
        {
            case GameState.playerTurn:
                break;

            case GameState.enemyTurn:
                //Debug.Log("enemy moving");
                Invoke(nameof(EnemyTurn), 1.0f);;
                ChangeGameState(GameState.playerTurn);
                //Debug.Log("enemy done");
                break;
        }
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
