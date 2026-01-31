using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentGameState;

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
                Debug.Log("enemyturn");
                break;
        }
    }

    public enum GameState
    {
        playerTurn = 0,
        enemyTurn = 1
    }
}
