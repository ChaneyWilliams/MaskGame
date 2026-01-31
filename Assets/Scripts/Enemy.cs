using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;
    public Vector2 targetPosition;
    public float direction = 1.0f;

    private bool isMoving = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;
        EnemyManager.instance.enemies.Add(this);
    }

    public void StartMove()
    {
        UnityEngine.Debug.Log("I am Moving");
        isMoving = true;
        targetPosition = rb.position + Vector2.right * direction;
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime));

        if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
        {
            isMoving = false;
        }
    }

}
