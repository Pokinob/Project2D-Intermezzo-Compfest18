using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System;

public class DialogueUI : MonoBehaviour
{
    Story story;

    [SerializeField]
    private GameObject dialoguePanel;
    [SerializeField]
    private TMP_Text speakerText;
    [SerializeField]
    private TMP_Text mainText;
    [SerializeField]
    private PlayerOverworld _player;

    void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void AdvanceDialogue()
    {
        if (story.canContinue)
        {
            mainText.text = story.Continue();
        }
        else
        {
            _player.playerState = PlayerState.Idle;
            dialoguePanel.SetActive(false);
        }
    }

    public void RunDialogue(TextAsset inkAsset)
    {
        _player.playerState = PlayerState.Interacting;
        story = new Story(inkAsset.text);
        dialoguePanel.SetActive(true);

        mainText.text = story.Continue();
    }
}
