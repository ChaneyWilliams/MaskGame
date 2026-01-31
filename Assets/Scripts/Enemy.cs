using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public List<Transform> path;
    public float speed = 5f;

    private int index = 0;
    private bool isMoving;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        EnemyManager.instance.enemies.Add(this);
    }

    public void StartMove()
    {
        isMoving = true;
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        Vector3 target = path[index].position;

        rb.MovePosition(
            Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime)
        );

        if (Vector3.Distance(rb.position, target) < 0.01f)
        {
            index++;

            if (index >= path.Count)
            {
                index = 0;
            }
            isMoving = false;
        }
    }
}
