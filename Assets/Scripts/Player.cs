using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    public static Player instance;
    private Rigidbody2D rb;
    public PlayerState currentPlayerState;
    public float speed = 5.0f;
    public Vector3 targetPosition;
    public Animator animator;
    public bool stuck = false;

    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;
    }
    void FixedUpdate()
    {
        if (!animator.GetBool("isMoving")) return;

        rb.MovePosition(Vector3.MoveTowards(rb.position, targetPosition, speed * Time.fixedDeltaTime));

        if (Vector3.Distance(rb.position, targetPosition) < 0.01f)
        {
            rb.position = targetPosition;
            animator.SetBool("isMoving", false);
            GameManager.instance.GetTile(gameObject);
            GameManager.instance.ChangeGameState(GameManager.GameState.enemyTurn);
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        if (GameManager.instance.currentGameState == GameManager.GameState.enemyTurn || animator.GetBool("isMoving")) return;
        if (stuck)
        {
            Debug.Log("Youre stuck womp womp");
            GameManager.instance.ChangeGameState(GameManager.GameState.enemyTurn);
            stuck = false;
            return;
        }

        Vector2 input = context.ReadValue<Vector2>();
        if (input.x > 0)
        {
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else if (input.x < 0)
        {
            gameObject.transform.localScale = new Vector3(-Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }

        if (Mathf.Abs(input.x) == 1f)
        {
            targetPosition = transform.position + new Vector3(input.x, 0f, 0f);
            animator.SetBool("isMoving", true);
        }
        else if (Mathf.Abs(input.y) == 1f)
        {
            targetPosition = transform.position + new Vector3(0f, input.y, 0f);
            animator.SetBool("isMoving", true);
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
    public void ChangeToNormal(InputAction.CallbackContext context)
    {
        if (currentPlayerState != PlayerState.NormalState)
        {
            Debug.Log("Changing to Normal");
            ChangePlayerState(PlayerState.NormalState);
        }
    }
    public void ChangeToFire(InputAction.CallbackContext context)
    {
        if (currentPlayerState != PlayerState.FireState)
        {
            Debug.Log("Changing to Fire");
            ChangePlayerState(PlayerState.FireState);
        }
    }
    public void ChangeToEarth(InputAction.CallbackContext context)
    {
        if (currentPlayerState != PlayerState.EarthState)
        {
            Debug.Log("Changing to Earth");
            ChangePlayerState(PlayerState.EarthState);
        }
    }
    public void ChangeToWater(InputAction.CallbackContext context)
    {
        if (currentPlayerState != PlayerState.WaterState)
        {
            Debug.Log("Changing to Water");
            ChangePlayerState(PlayerState.WaterState);
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
        GameManager.instance.ChangeGameState(GameManager.GameState.enemyTurn);
    }

    public enum PlayerState
    {
        NormalState = 0,
        EarthState = 1,
        FireState = 2,
        WaterState = 3

    }

}
