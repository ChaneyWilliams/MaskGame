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

        switch (currentGameState)
        {
            case GameState.playerTurn:
                break;

            case GameState.enemyTurn:
                //Debug.Log("enemy moving");
                Invoke(nameof(ThisIsDumb), 5.0f);
                EnemyManager.instance.TakeTurn();
                //Debug.Log("enemy done");
                break;
        }
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

    public void ThisIsDumb()
    {
        Debug.Log("oh my god");
    }

    public enum GameState
    {
        playerTurn = 0,
        enemyTurn = 1
    }
}
