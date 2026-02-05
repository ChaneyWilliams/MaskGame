using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Debug = UnityEngine.Debug;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    public static Player instance;
    private Rigidbody2D rb;
    public PlayerState currentPlayerState;
    public float speed = 5.0f;
    public Vector3 moveTargetPos;
    public Animator animator;
    public bool stuck = false;
    public bool gameOver = false;
    TileBase currentTile;
    Vector3 pickUpPos;


    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        moveTargetPos = transform.position;
    }
    void FixedUpdate()
    {
        if (!animator.GetBool("isMoving")) return;
        //SoundEffectManager.Play("Gravel_Footsteps"); NOT HERE calls like 70 times
        rb.MovePosition(Vector3.MoveTowards(rb.position, moveTargetPos, speed * Time.fixedDeltaTime));

        if (Vector3.Distance(rb.position, moveTargetPos) < 0.01f)
        {
            rb.position = moveTargetPos;
            animator.SetBool("isMoving", false);
            TileData currentTile = GameManager.instance.GetTile(gameObject.transform.position);
            MapManager.instance.TileChoices(currentTile, gameObject);
            GameManager.instance.ChangeGameState(GameManager.GameState.EnemyTurn);
        }
    }


    public void Move(InputAction.CallbackContext context)
    {

        if (GameManager.instance.currentGameState == GameManager.GameState.EnemyTurn || animator.GetBool("isMoving") || gameOver) return;
        if (stuck)
        {
            GameManager.instance.ChangeGameState(GameManager.GameState.EnemyTurn);
            stuck = false;
            return;
        }
        SoundEffectManager.Play("Gravel_Footsteps");
        Vector3 oldTargetPosition = moveTargetPos;
        Vector2 input = context.ReadValue<Vector2>();

        if (Mathf.Abs(input.x) == 1f)
        {
            moveTargetPos = transform.position + new Vector3(input.x, 0f, 0f);
            pickUpPos = moveTargetPos + new Vector3(input.x, 0f, 0f);
            if (GameManager.instance.GetTile(moveTargetPos) == null || GameManager.instance.GetTile(moveTargetPos).tileState == TileData.TileState.WallTile)
            {
                moveTargetPos = oldTargetPosition;
                pickUpPos = moveTargetPos + new Vector3(0f, input.y, 0f);
                return;
            }
            animator.SetBool("isMoving", true);
            gameObject.transform.localScale = (input.x < 0) ?
            new Vector3(-Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z) :
            new Vector3(Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else if (Mathf.Abs(input.y) == 1f)
        {
            moveTargetPos = transform.position + new Vector3(0f, input.y, 0f);
            pickUpPos = moveTargetPos + new Vector3(0f, input.y, 0f);
            if (GameManager.instance.GetTile(moveTargetPos) == null || GameManager.instance.GetTile(moveTargetPos).tileState == TileData.TileState.WallTile)
            {
                moveTargetPos = oldTargetPosition;
                pickUpPos = moveTargetPos + new Vector3(0f, input.y, 0f);
                return;
            }
            animator.SetBool("isMoving", true);
        }
    }
    public void PickUpTile(InputAction.CallbackContext context)
    {
        TileData tile = GameManager.instance.GetTile(pickUpPos);
        if (currentTile == null)
        {
            if (tile == null) return;
            switch (tile.tileState)
            {
                case TileData.TileState.EarthTile:
                    currentTile = MapManager.instance.GetTileBase(0);
                    break;

                case TileData.TileState.FireTile:
                    currentTile = MapManager.instance.GetTileBase(1);
                    break;

                case TileData.TileState.WaterTile:
                    currentTile = MapManager.instance.GetTileBase(2);
                    break;

                case TileData.TileState.NormalTile:
                    currentTile = MapManager.instance.GetTileBase(3);
                    break;

                default:
                    return;
            }

            MapManager.instance.map.SetTile(Vector3Int.FloorToInt(pickUpPos), null);
        }
        else
        {
            if (tile == null || tile.tileState != TileData.TileState.WallTile)
            {
                MapManager.instance.map.SetTile(Vector3Int.FloorToInt(pickUpPos), currentTile);
                currentTile = null;
            }
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

    public void Reset(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.instance.ResetLevel();
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
        GameManager.instance.ChangeGameState(GameManager.GameState.EnemyTurn);
    }

    public enum PlayerState
    {
        NormalState = 0,
        EarthState = 1,
        FireState = 2,
        WaterState = 3

    }

}
