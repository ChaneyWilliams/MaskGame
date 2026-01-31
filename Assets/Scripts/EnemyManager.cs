using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

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
            UnityEngine.Debug.Log("moving enemies");
            enemy.StartMove();
        }
    }

}
