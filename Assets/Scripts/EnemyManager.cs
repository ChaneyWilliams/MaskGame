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
            instance = this;;
        }
        else { Destroy(gameObject); }
    }

    public void TakeTurn()
    {
        foreach(Enemy enemy in enemies)
        {
            //UnityEngine.Debug.Log("moving enemies");
            enemy.StartMove();
        }
    }

}
