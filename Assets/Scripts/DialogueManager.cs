using Ink.Runtime;
using Ink.UnityIntegration;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour, IDataPersistence
{
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private Animator layoutAnimator;
    public PlayableDirector beforeStartGame;
    public PlayableDirector fadeInScene;
    public PlayableDirector fadeOutScene;


    [Header("Choices UI")]
    [SerializeField] private Transform choiceTransform;
    [SerializeField] private GameObject choicePrefab;

    [Header("Ink Settings")]
    private Story currentStory;
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    public DialogueVariable dialogueVariables { get; private set; }
    [SerializeField] private InkFile loadglobalsInkFile;
    [SerializeField] private PlayableDirector introScene;


    [Header("Other")]
    public bool dialogueIsPlaying { get; private set; }
    public bool canContinue;
    public bool canBattle=false;
    public string namaMc = "???";
    private bool isTyping;
    private string currentText;
    public string isClaim;
    public string ItemName;
    public string ItemType;
    [SerializeField]private TextMeshProUGUI inputNama;
    [SerializeField] private GameObject inputPanel;
    [SerializeField] private GameObject playerPosTutorial;
    [SerializeField] private List<GameObject> enemyPosTutorial;
    private Coroutine typingCoroutine;
    private Coroutine ExitDialogueCoroutine;
    public Coroutine ClaimCoroutine;
    private static DialogueManager instance;


    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("Double Instance of DialogueManager found!");
            return;
        }
        instance = this;
        isClaim = "";
        //beforeStartGame.Play();
        StartCoroutine(showMainPanel());
    }

    public void ResetTimeline(PlayableDirector timeline)
    {
        timeline.gameObject.SetActive(true);
        timeline.Play();
    }
    public void startGame()
    {
        //ResetTimeline(fadeInScene);
        //StartCoroutine(loadingGame());
        Debug.Log(loadglobalsInkFile);
        dialogueVariables = new DialogueVariable(loadglobalsInkFile);
    }

    IEnumerator showMainPanel()
    {
        yield return new WaitForSeconds(0.2f);
        PanelManager.GetInstance().mainPanel.SetActive(true);
    }
    IEnumerator loadingGame()
    {
        yield return new WaitUntil(() => fadeInScene.state != PlayState.Playing);
        beforeStartGame.Stop();
        fadeOutScene.Play();
        yield return new WaitForSeconds(0.2f);
        introScene.Play();
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        isTyping = false;
        dialogueIsPlaying = false;
        canContinue = true;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if (!dialogueIsPlaying) return;
        if (!canContinue) return;
        
        if(InputManager.GetInstance().GetSubmitPressed() && currentStory.currentChoices.Count == 0)
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(InkFile inkJSON)
    {   
        //Debug.Log("Enter Dialogue Mode");
        currentStory = new Story(inkJSON.storyJson);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        dialogueVariables.StartListening(currentStory);
        // Cara manggil function dari ink ke unity, misal di ink ada {playDebug("test")} maka akan memanggil function playDebug di unity
        BindInkExternalFunction();
        canContinue = true;
        dialogueText.text = "";
        //Debug.Log("Before Continue Story");
        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitUntil(() => isClaim == "");
        dialogueVariables.StopListening(currentStory);
        UnBindInkExternalFunction();
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        ExitDialogueCoroutine = null;
    }


    private void ContinueStory()
    {
        //Debug.Log("Continue Story");
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
            //Debug.Log("Continue Story");
            currentText = currentStory.Continue();
            if(currentStory.currentTags.Count > 0)
            {
                StartCoroutine(HandleTags(currentStory.currentTags));
            }
            //while (currentStory.canContinue && string.IsNullOrEmpty(currentText))
            //{
            //    Debug.Log("Continue Story");
            //    //Debug.Log(currentStory.Continue());
            //    currentText = currentStory.Continue();
            //    Debug.Log("Current Text: " + currentText);
            //    if (currentStory.currentTags.Count > 0)
            //    {
            //        Debug.Log("Tags: " + string.Join(", ", currentStory.currentTags));
            //        StartCoroutine(HandleTags(currentStory.currentTags));
            //        //Debug.Log("This is Tag");
            //    }
            //}

            if (string.IsNullOrEmpty(currentText) && !currentStory.canContinue)
            {
                //Debug.Log("Finish");
                ExitDialogueCoroutine = StartCoroutine(ExitDialogueMode());
                return;
            }

            Debug.Log("Continue Story2");
            typingCoroutine = StartCoroutine(DisplayText(currentText));

        }
        else
        {
            //Debug.Log("Finish");
            ExitDialogueCoroutine = StartCoroutine(ExitDialogueMode());
        }
    }

    private void BindInkExternalFunction()
    {
        currentStory.BindExternalFunction("GetItem", () =>
        {
            return ItemName;
        });

        currentStory.BindExternalFunction("DelayClaim", () =>{
            ClaimCoroutine = StartCoroutine(DelayClaim());
        });

        currentStory.BindExternalFunction("OpenGate", (int levelIndex) =>
        {
            GateManager.GetInstance().SetGateLevel(levelIndex);
        });

        currentStory.BindExternalFunction("playQuest", (string Quest) =>
        {
            Debug.Log(Quest);
            switch (Quest)
            {
                case "1":
                    QuestManager.GetInstance().startQ1();
                    break;
                case "7":
                    QuestManager.GetInstance().startP7();
                    break;
                default:
                    Debug.Log("Quest Not Found");
                    break;
            }
        });
        currentStory.BindExternalFunction("inputName", () =>
        {
            inputPanel.gameObject.SetActive(true);
            PlayerOverworld.GetInstance().isFreeze = true;
            canContinue = false;
        });
        currentStory.BindExternalFunction("startTutorial", () =>
        {
            introManager.GetInstance().onWASDPressed();
        });
        currentStory.BindExternalFunction("continueTimeline", () => 
        {
            if (timelineManager.GetInstance().currentTimeline != null)
            {
                canContinue = false;
                dialoguePanel.SetActive(false);
                //timelineManager.GetInstance().currentTimeline.Play();
                timelineManager.GetInstance().currentTimeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
            }
        });
        currentStory.BindExternalFunction("StartBattle", (int enemyIndex) =>
        {
            BattleStats[] temp = new BattleStats[1];
            temp[0] = EnemyDB.GetInstance().enemyDataArray[enemyIndex];
            canContinue = false;
            BattleManager.GetInstance().StartBattle(temp, enemyPosTutorial, playerPosTutorial);
        });
        currentStory.BindExternalFunction("ContinueBattle", (bool value) =>
        {
            showDialogueBattle(value);
        });
        currentStory.BindExternalFunction("AddItem", (string itemID, int itemCnt) =>
        {
            //Debug.Log("Add Item: " + itemID + " Count: " + itemCnt);
            inventoryManager.GetInstance().AddItem(itemID, itemCnt);
        });
    }

    private void UnBindInkExternalFunction()
    {
        currentStory.UnbindExternalFunction("GetItem");
        currentStory.UnbindExternalFunction("OpenGate");
        currentStory.UnbindExternalFunction("DelayClaim");
        currentStory.UnbindExternalFunction("playQuest");
        currentStory.UnbindExternalFunction("inputName");
        currentStory.UnbindExternalFunction("startTutorial");
        currentStory.UnbindExternalFunction("continueTimeline");
        currentStory.UnbindExternalFunction("StartBattle");
        currentStory.UnbindExternalFunction("AddItem");
    }

    private void showDialogueBattle(bool value)
    {
        if (value)
        {
            canBattle = true;
            dialoguePanel.SetActive(false);
            canContinue = false;
        }
        else
        {
            canBattle = false;
            dialoguePanel.SetActive(true);
            canContinue = true;
        }
    }

    IEnumerator DelayClaim()
    {
        yield return new WaitUntil(() => ExitDialogueCoroutine !=null);
        isClaim = "";
        ItemName = "";
        ClaimCoroutine = null;
    }

    IEnumerator DisplayText(string currentText)
    {
        yield return new WaitUntil(() => canContinue);
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
    private IEnumerator HandleTags(List<string> tags)
    {
        yield return new WaitUntil(() => canContinue);
        //Debug.Log("Handle Tags");
        foreach (string tag in tags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                //Debug.LogWarning("Tag could not be parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            Debug.Log("Tag Key: " + tagKey + ", Tag Value: " + tagValue);
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    {
                        if (tagValue == "???")
                        {
                            if (((Ink.Runtime.StringValue)dialogueVariables.variableDictionary["MCName"]).value != "???")
                            {
                                Debug.Log("Ada nama");
                                namaMc = ((Ink.Runtime.StringValue)dialogueVariables.variableDictionary["MCName"]).value;
                            }
                            if (namaMc != "???")
                            {
                                //Debug.Log("Tidak ada nama");
                                speakerNameText.text = namaMc;
                            }
                            else
                            {
                                Debug.Log("Blm ada nama");
                                speakerNameText.text = "???";
                            }
                        }
                        else
                        {
                            speakerNameText.text = tagValue;
                        }
                        break;
                    }
                case PORTRAIT_TAG:
                    {
                        Debug.Log("changeImg: " + tagValue);
                        portraitAnimator.Play(tagValue);
                        break;
                    }
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;
                default:
                    //Debug.LogWarning("Tag is not recognized: " + tag);
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

    public void InputName()
    {
        if (inputNama.text == null) return;

        namaMc = inputNama.text;
        Ink.Runtime.Object val = new Ink.Runtime.StringValue(namaMc);
        dialogueVariables.variableDictionary.Remove("MCName");
        dialogueVariables.variableDictionary.Add("MCName",  new Ink.Runtime.StringValue(namaMc));
        dialogueVariables.forceVariable(currentStory,"MCName", val);
        canContinue = true;
        PlayerOverworld.GetInstance().isFreeze = false;
        inputPanel.SetActive(false);
    }
    public void showDialogue()
    {
        if (currentStory!=null)
        {
            timelineManager.GetInstance().currentTimeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
            //timelineManager.GetInstance().currentTimeline.Evaluate();
            //timelineManager.GetInstance().currentTimeline.Pause();
            dialoguePanel.SetActive(true);
            canContinue = true;
        }
    }

    public void startDialogueTimeline(InkFile ink)
    {
        timelineManager.GetInstance().currentTimeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
        //timelineManager.GetInstance().currentTimeline.Evaluate();
        //timelineManager.GetInstance().currentTimeline.Pause();
        EnterDialogueMode(ink);
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
