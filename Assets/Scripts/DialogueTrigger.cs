using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public TextAsset inkAsset;

    DialogueUI dialogueUI;

    void Start()
    {
        GameObject dialogueObject = GameObject.FindGameObjectWithTag("DialogueUI");
        dialogueUI = dialogueObject.GetComponent<DialogueUI>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            dialogueUI.RunDialogue(inkAsset);
        }
    }
}
