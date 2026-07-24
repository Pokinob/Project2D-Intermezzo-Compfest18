using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TriggerSavePlace : MonoBehaviour
{
    [Header("Save Place Settings")]
    private bool playerInRange;
    [SerializeField] private PanelManager panelManager;

    private void Awake()
    {
        playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            if (InputManager.GetInstance().GetInteractPressed())
            {
                panelManager.OpenSavePanel();
            }
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
