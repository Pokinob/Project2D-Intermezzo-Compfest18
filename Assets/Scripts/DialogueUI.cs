using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueUI : MonoBehaviour
{
    Story story;

    public TMP_Text speakerText;
    public TMP_Text mainText;
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(AdvanceDialogue);

        gameObject.SetActive(false);
    }

    void AdvanceDialogue()
    {
        if (story.canContinue)
        {
            mainText.text = story.Continue();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void RunDialogue(TextAsset inkAsset)
    {
        story = new Story(inkAsset.text);
        gameObject.SetActive(true);

        mainText.text = story.Continue();
    }
}
