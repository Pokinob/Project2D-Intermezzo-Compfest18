using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTrigger : MonoBehaviour
{
    public GameObject Object;
    [SerializeField]
    private DialogueUI dialogueUI;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col == null) return;
        if(col.CompareTag("Interactable"))
            Object = col.gameObject;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col == null) return;
        Object = null;
    }

    public void TriggerSystem(PlayerState playerState)
    {

        if(Object.GetComponent<dialogueData>() != null)
        {
            if(playerState == PlayerState.Idle)
            dialogueUI.RunDialogue(Object.GetComponent<dialogueData>().inkAsset);
            else
            {
                dialogueUI.AdvanceDialogue();
            }
        }
    }
}
