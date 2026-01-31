using UnityEngine;
public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform[] path;
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
        UnityEngine.Debug.Log("moving");
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

            if (index >= path.Length)
            {
                index = 0;
            }
            isMoving = false;
        }
    }
}
