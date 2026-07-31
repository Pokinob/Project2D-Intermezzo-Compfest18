using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerOverworld : MonoBehaviour, IDataPersistence
{

    public float moveSpeed;

    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    private bool isFreeze;

    [SerializeField]
    private Animator animator;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying) return;

        updateMove();
    }

    #region Movement

    private void updateMove()
    {
        Vector2 moveDirection = InputManager.GetInstance().GetMoveDirection();
        if (moveDirection != Vector2.zero)
        {
            rigidBody.MovePosition(rigidBody.position +
                moveDirection *
                moveSpeed *
                Time.fixedDeltaTime);

            animator.SetBool("IsWalking", true);
            animator.SetFloat("DirectionX", moveDirection.x);
            animator.SetFloat("DirectionY", moveDirection.y);
        }
        else
        {
            animator.SetFloat("LastDirectionX", animator.GetFloat("DirectionX"));
            animator.SetFloat("LastDirectionY", animator.GetFloat("DirectionY"));
            animator.SetBool("IsWalking", false);
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveSpeed += 2f;
        }
        else if (context.canceled)
        {
            moveSpeed -= 2f;
        }
    }

    public void LoadData(GameData data)
    {
        rigidBody.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = rigidBody.transform.position;
    }



    #endregion
}
