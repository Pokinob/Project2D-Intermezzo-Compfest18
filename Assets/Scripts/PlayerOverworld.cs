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


    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
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
