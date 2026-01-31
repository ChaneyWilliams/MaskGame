using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public List<Enemy> enemies;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    public void TakeTurn()
    {
        foreach(Enemy enemy in enemies)
        {
            enemy.StartMove();
        }
        GameManager.instance.ChangeGameState(GameManager.GameState.playerTurn);
    }

}
