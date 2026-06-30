using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerOverworld : MonoBehaviour
{

    public float walkSpeed;
    public float walkSpeedLossOnStop;

    Vector2 moveDirection;
    Rigidbody2D rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (moveDirection != Vector2.zero)
        {
            rigidBody.linearVelocity = moveDirection * walkSpeed;
            rigidBody.linearDamping = 0;
        }
        else
        {
            rigidBody.linearDamping = walkSpeedLossOnStop;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }
}
