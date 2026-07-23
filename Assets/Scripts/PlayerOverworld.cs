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

    [SerializeField]
    private Rigidbody2D rigidBody;
    
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
        if(DialogueManager.GetInstance().dialogueIsPlaying) return;

        updateMove();
    }

    #region Movement

    private void updateMove()
    {
        if (InputManager.GetInstance().GetMoveDirection() != Vector2.zero)
        {
            rigidBody.MovePosition(rigidBody.position +
                InputManager.GetInstance().GetMoveDirection() *
                moveSpeed *
                Time.fixedDeltaTime);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveSpeed += 2f;
        }
        else if(context.canceled)
        {
            moveSpeed -= 2f;
        }
    }

    
    #endregion
}
