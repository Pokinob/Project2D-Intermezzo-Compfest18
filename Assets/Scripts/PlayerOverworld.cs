using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class PlayerOverworld : MonoBehaviour, IDataPersistence
{

    public float moveSpeed;

    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    private bool isFreeze=false;

    [SerializeField]
    private Animator animator;

    private Vector2 moveDirection;

    public void EndTimeline()
    {
        StartCoroutine(WaitForEndTimeline());
    }

    private IEnumerator WaitForEndTimeline()
    {
        moveDirection = Vector2.zero;
        yield return new WaitForSeconds(0.1f);
        isFreeze = false;
    }

    public void StartTimeline() 
    {
        isFreeze = true;
    }

    private void Awake()
    {
        moveDirection = Vector2.zero;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying || isFreeze) return;

        updateMove();
    }

    #region Movement

    private void updateMove()
    {
        moveDirection = InputManager.GetInstance().GetMoveDirection();
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

    #region Animation On timeline
    public void FaceUp()
    {
        //Debug.Log("FaceUp");
        animator.SetBool("IsWalking", false);
        animator.SetFloat("DirectionX", 0);
        animator.SetFloat("DirectionY", 1);
        animator.SetFloat("LastDirectionX", 0);
        animator.SetFloat("LastDirectionY", 1);
    }
    public void FaceDown()
    {
        //Debug.Log("FaceDown");
        animator.SetBool("IsWalking", false);
        animator.SetFloat("DirectionX", 0);
        animator.SetFloat("DirectionY", -1);
        animator.SetFloat("LastDirectionX", 0);
        animator.SetFloat("LastDirectionY", -1);
    }
    public void FaceLeft()
    {
        animator.SetBool("IsWalking", false);
        animator.SetFloat("DirectionX", -1);
        animator.SetFloat("DirectionY", 0);
        animator.SetFloat("LastDirectionX", -1);
        animator.SetFloat("LastDirectionY", 0);
    }
    public void FaceRight()
    {
        animator.SetBool("IsWalking", false);
        animator.SetFloat("DirectionX", 1);
        animator.SetFloat("DirectionY", 0);
        animator.SetFloat("LastDirectionX", 1);
        animator.SetFloat("LastDirectionY", 0);
    }
    #endregion
}
