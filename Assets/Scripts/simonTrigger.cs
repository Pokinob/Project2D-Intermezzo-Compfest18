using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class simonTrigger : MonoBehaviour
{
    private bool playerInRange;
    [SerializeField] private int buttonIndex;

    private void Awake()
    {
        playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && InputManager.GetInstance().GetInteractPressed())
        {
            SimonSays.GetInstance().checkBtn(buttonIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
