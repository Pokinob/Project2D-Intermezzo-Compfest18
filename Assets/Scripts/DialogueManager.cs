using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private Transform choiceTransform;
    [SerializeField] private GameObject choicePrefab;
    

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }
    public bool isChoiceDisplayed { get; private set; }
    private static DialogueManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of DialogueManager found!");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        isChoiceDisplayed = false;
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!dialogueIsPlaying) return;
        
        if(InputManager.GetInstance().GetSubmitPressed())
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();

            //display choices, if any, for this dialogue line
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void clearChoices()
    {
        foreach(Transform childChoice in choiceTransform)
        {
            Destroy(childChoice.gameObject);
        }
    }

    private void DisplayChoices()
    {
        clearChoices();
        List<Choice> currentChoices = currentStory.currentChoices;
        if(currentChoices.Count > 0)
        {
            isChoiceDisplayed = true;
            for (int i = 0; i < currentChoices.Count; i++)
            {
                int choiceIndex = i;
                createChoices(() => MakeChoice(choiceIndex), currentChoices, choiceIndex);
            }
        }
    }

    private void createChoices(UnityEngine.Events.UnityAction onClick, List<Choice> currentChoices, int choiceIndex)
    {
        GameObject choiceButton = Instantiate(choicePrefab, choiceTransform);
        Button btn = choiceButton.GetComponent<Button>();
        //Debug.Log(btn);
        choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = currentChoices[choiceIndex].text;
        btn.onClick.AddListener(onClick);
    }

    public void MakeChoice(int choiceIndex)
    {
        //Debug.Log("choice selected: " + choiceIndex);
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
        isChoiceDisplayed = false;
    }
}
