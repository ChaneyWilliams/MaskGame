using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;
    public Vector2 targetPosition;
    public float direction = 1.0f;

    public bool isMoving = false;
    public bool stuck = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;
        EnemyManager.instance.enemies.Add(this);
    }

    public void StartMove()
    {
        if (!stuck)
        {
            isMoving = true;
            if (rb == null) return;
            targetPosition = rb.position + Vector2.right * direction;
        }
        else
        {
            stuck = false;   
        }
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime));

        if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
        {
            isMoving = false;
            GameManager.instance.GetTile(gameObject);
        }
    }

}
