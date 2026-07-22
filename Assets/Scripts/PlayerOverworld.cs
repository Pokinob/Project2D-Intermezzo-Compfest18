using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public enum PlayerState
{
    Idle,
    Interacting
}
public class PlayerOverworld : MonoBehaviour
{

    public float moveSpeed;

    Vector2 moveDirection;

    [SerializeField]
    private Rigidbody2D rigidBody;
    
    [SerializeField]
    private PlayerTrigger playerTrigger;

    [SerializeField]
    private bool isFreeze;

    public PlayerState playerState;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerState = PlayerState.Idle;
    }

    void FixedUpdate()
    {
        if (playerState == PlayerState.Interacting) return; 
        rigidBody.MovePosition(rigidBody.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    #region PlayerInput
    public void OnMove(InputAction.CallbackContext context)
    {
        if (isFreeze) return;
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

    public void OnInteract(InputAction.CallbackContext context)
    {
        //Debug.Log("Try Interact");
        if (context.performed)
        {
            //Debug.Log("Try Interact");
            if (playerTrigger.Object == null) return;
                Debug.Log("Interacting with " + playerTrigger.Object.name);
                playerTrigger.TriggerSystem(playerState);
        }
    }
    #endregion
}
