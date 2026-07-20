using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOverworld : MonoBehaviour
{

    public float moveSpeed;

    Vector2 moveDirection;
    Rigidbody2D rigidBody;



    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    #region PlayerInput
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else if(context.canceled)
        {
            moveDirection = Vector2.zero;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Sprint");
            moveSpeed += 2f;
        }
        else if(context.canceled)
        {
            moveSpeed -= 2f;
        }
    }
    #endregion
}
