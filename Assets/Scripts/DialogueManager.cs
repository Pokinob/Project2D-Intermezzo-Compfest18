using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour, IDataPersistence
{
    [Header("Dialogue UI")]

    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    private Animator layoutAnimator;


    [Header("Choices UI")]
    [SerializeField] private Transform choiceTransform;
    [SerializeField] private GameObject choicePrefab;

    [Header("Ink Settings")]
    private Story currentStory;
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private DialogueVariable dialogueVariables;
    [SerializeField] private TextAsset loadglobalsInkFile;


    [Header("Other")]
    public bool dialogueIsPlaying { get; private set; }
    private bool isTyping;
    private string currentText;
    private Coroutine typingCoroutine;
    private static DialogueManager instance;


    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of DialogueManager found!");
        }
        instance = this;
    }

    public void startGame()
    {
        dialogueVariables = new DialogueVariable(loadglobalsInkFile);
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        isTyping = false;
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        layoutAnimator = dialoguePanel.GetComponent<Animator>();
    }

    private void Update()
    {
        if (!dialogueIsPlaying) return;
        
        if(InputManager.GetInstance().GetSubmitPressed() && currentStory.currentChoices.Count == 0)
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        dialogueVariables.StartListening(currentStory);
        // Cara manggil function dari ink ke unity, misal di ink ada {playDebug("test")} maka akan memanggil function playDebug di unity
        currentStory.BindExternalFunction("playDebug", (string debug) => {
            Debug.Log(debug);
        });

        dialogueText.text = "";
        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueVariables.StopListening(currentStory);
        currentStory.UnbindExternalFunction("playDebug");
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }


    private void ContinueStory()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentText;
            isTyping = false;

            if(currentStory.currentChoices.Count > 0)
                DisplayChoices();

            return;
        }
        if (currentStory.canContinue)
        {
            currentText = currentStory.Continue();

            if (currentStory.currentTags.Count > 0)
                HandleTags(currentStory.currentTags);

            //Debug.Log("Continue Story");
            typingCoroutine = StartCoroutine(DisplayText(currentText));

        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    IEnumerator DisplayText(string currentText)
    {
        dialogueText.text = "";
        isTyping = true;
        foreach (char letter in currentText)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        isTyping = false;
        DisplayChoices();
    }


    // Handle tags from Ink story
    private void HandleTags(List<string> tags)
    {
        foreach (string tag in tags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogWarning("Tag could not be parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    speakerNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    // Handle portrait change (soon)
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;

                default:
                    Debug.LogWarning("Tag is not recognized: " + tag);
                    break;
            }
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
        List<Choice> currentChoices = currentStory.currentChoices;
        clearChoices();
        for (int i = 0; i < currentChoices.Count; i++)
        {
            int choiceIndex = i;
            createChoices(() => MakeChoice(choiceIndex), currentChoices, choiceIndex);
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
        clearChoices();
        ContinueStory();
    }

    public void LoadData(GameData data)
    {
        dialogueVariables = new DialogueVariable(loadglobalsInkFile, data.inkData);
    }

    public void SaveData(ref GameData data)
    {
        dialogueVariables.SaveData(ref data);
    }
}
