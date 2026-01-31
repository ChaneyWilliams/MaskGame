using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public PlayerState currentPlayerState;
    public float speed = 5.0f;
    Vector3 targetPosition;
    bool isMoving = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;
    }
    void FixedUpdate()
    {
        if (!isMoving) return;

        rb.MovePosition(Vector3.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime));

        if (Vector3.Distance(rb.position, targetPosition) < 0.01f)
        {
            rb.position = targetPosition;
            isMoving = false;
            GameManager.instance.GetTile(gameObject);
            GameManager.instance.ChangeGameState(GameManager.GameState.enemyTurn);
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        if (GameManager.instance.currentGameState == GameManager.GameState.enemyTurn) return;
        if (isMoving) return;

        Vector2 input = context.ReadValue<Vector2>();

        if (Mathf.Abs(input.x) == 1f)
        {
            targetPosition = transform.position + new Vector3(input.x, 0f, 0f);
            isMoving = true;
        }
        else if (Mathf.Abs(input.y) == 1f)
        {
            targetPosition = transform.position + new Vector3(0f, input.y, 0f);
            isMoving = true;
        }
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (GameManager.instance.GameIsPaused)
        {
            GameManager.instance.Resume();
        }
        else
        {
            GameManager.instance.Paused();
        }
    }

    public void ChangePlayerState(PlayerState newState)
    {
        currentPlayerState = newState;
        Debug.Log(currentPlayerState);

        switch (currentPlayerState)
        {
            case PlayerState.NormalState:
                break;
            case PlayerState.EarthState:
                break;
            case PlayerState.FireState:
                break;
            case PlayerState.WaterState:
                break;
        }
    }

    public enum PlayerState
    {
        NormalState = 0,
        EarthState = 1,
        FireState = 2,
        WaterState = 3

    }

}
