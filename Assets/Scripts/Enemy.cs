using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;
    public Vector2 targetPosition;
    public float direction = 1.0f;
    public bool vertical;

    public bool isMoving = false;
    public bool stuck = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = rb.position;
        EnemyManager.instance.enemies.Add(this);
    }

    void FixedUpdate()
    {
        if (!isMoving) return;

        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime));

        if (Vector2.Distance(rb.position, targetPosition) < 0.01f)
        {
            isMoving = false;
            TileData currentTile = GameManager.instance.GetTile(gameObject.transform.position);
            MapManager.instance.TileChoices(currentTile, gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }

    public void StartMove()
    {
        if (!stuck)
        {
            Vector2 oldTargetPos = targetPosition;
            isMoving = true;
            if (rb == null) return;
            //toggles up and down or left to right
            if (vertical)
            {
                targetPosition = rb.position + Vector2.up * direction;
            }
            else
            {
                targetPosition = rb.position + Vector2.right * direction;
            }
            
            if (GameManager.instance.GetTile(targetPosition) == null)
            {
                targetPosition = oldTargetPos;
                gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                direction = -direction;
                return;
            }

        }
        else
        {
            stuck = false;
        }
    }
}
