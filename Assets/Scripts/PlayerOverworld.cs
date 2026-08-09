using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;


public class PlayerOverworld : MonoBehaviour, IDataPersistence
{

    public float moveSpeed;

    public Rigidbody2D rigidBody;

    [SerializeField]
    private Collider2D playerCollider;

    public bool isFreeze=false;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private LayerMask pushableLayer;

    private Vector2 moveDirection;

    private static PlayerOverworld instance;

    public void EndTimeline()
    {
        isFreeze = false;
    }

    public void StartTimeline() 
    {
        isFreeze = true;
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of PlayerOverworld found!");
            return;
        }
        instance = this;
        moveDirection = Vector2.zero;
    }

    public static PlayerOverworld GetInstance()
    {
        return instance;
    }

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying || isFreeze || InputManager.GetInstance().GetFreezeInput()) return;

        updateMove();
    }

    #region Movement

    private void updateMove()
    {
        moveDirection = InputManager.GetInstance().GetMoveDirection();
        if (moveDirection != Vector2.zero)
        {

            checkPush();
            
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

    public void forceMove(Vector2 targetPosition, float duration)
    {
        StartCoroutine(forceMoveCor(targetPosition, duration));
    }

    private IEnumerator forceMoveCor(Vector2 targetPosition, float duration)
    {
        animator.SetBool("IsWalking", true);
        animator.SetFloat("DirectionX", targetPosition.x);
        animator.SetFloat("DirectionY", targetPosition.y);
        float elapsedTime = 0f;
        while (elapsedTime < duration) {
            rigidBody.MovePosition(rigidBody.position +
            targetPosition *
            moveSpeed *
            Time.fixedDeltaTime);
            elapsedTime += Time.deltaTime;

            yield return new WaitForFixedUpdate();
        }
        animator.SetFloat("LastDirectionX", targetPosition.x);
        animator.SetFloat("LastDirectionY", targetPosition.y);
        animator.SetBool("IsWalking", false);
    }

    private void checkPush()
    {
        if ((moveDirection.x > 0 || moveDirection.x < 0) && (moveDirection.y > 0 || moveDirection.y < 0)) return;
        //if(moveDirection.x < 0 && moveDirection.y < 0) return;
        float offset = playerCollider.bounds.extents.x + 0.05f;

        if (Mathf.Abs(moveDirection.y) > 0)
            offset = playerCollider.bounds.extents.y + 0.05f;

        Vector2 checkPos = (Vector2)transform.position + moveDirection * offset;
        Collider2D hit = Physics2D.OverlapCircle(checkPos, 0.05f, pushableLayer);

        if (hit != null)
        {
            if (hit.GetComponent<Boulder>() != null)
            {
                hit.GetComponent<Boulder>()?.Push(moveDirection);
                return;
            }
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
