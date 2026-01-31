using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5.0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Mathf.Abs(context.ReadValue<Vector2>().x) == 1f)
            {
                Debug.Log(context.ReadValue<Vector2>().x);
                gameObject.transform.position += new Vector3(context.ReadValue<Vector2>().x, 0f, 0f);
            }

            if (Mathf.Abs(context.ReadValue<Vector2>().y) == 1f)
            {
                gameObject.transform.position += new Vector3(0f, context.ReadValue<Vector2>().y, 0f);
            }
        }
        else if (context.canceled)
        {
            return;
        }

    }
}
