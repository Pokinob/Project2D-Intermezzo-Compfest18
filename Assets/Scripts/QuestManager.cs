using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using Unity.VisualScripting;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    [SerializeField] private List<GameObject> objQuest1;
    [SerializeField] private List<GameObject> objQuest2;
    [SerializeField] private TextAsset inkFilesP2;
    [SerializeField] private TextAsset inkFilesP7;
    [SerializeField] private TextAsset inkFilesP4;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static QuestManager GetInstance()
    {
        return instance;
    }

    public void startQ1()
    {
        foreach (GameObject obj in objQuest1)
        {
            obj.SetActive(true);
        }
    }

    public void completedPuzzle2()
    {
        bool checkP2Obj = (DialogueManager.GetInstance().dialogueVariables.variableDictionary["P2Complete"] as Ink.Runtime.BoolValue).value;


        if (checkP2Obj)
        {
            //Debug.Log("P2 already completed, not starting dialogue");
            return;
        }
        else
        {
            int count = 0;
            foreach (GameObject obj in objQuest2)
            {
                if (obj.GetComponent<triggerBoulder>().isBoulderTriggered)
                {
                    count++;
                }
            }
            if (count == objQuest2.Count)
            {
                Debug.Log("All boulders triggered, starting dialogue for P2");
                DialogueManager.GetInstance().EnterDialogueMode(inkFilesP2);
            }
        }
        
    }

    public void startP4()
    {
        for (int i = 1; i <= 3; i++)
        {
            int ranDigit = Random.Range(0, 9);
            Ink.Runtime.Object value = new Ink.Runtime.IntValue(ranDigit);
            DialogueManager.GetInstance().dialogueVariables.variableDictionary.Remove($"P4Digit{i}");
            DialogueManager.GetInstance().dialogueVariables.variableDictionary.Add($"P4Digit{i}", value);
        }
    }

    public void startP7()
    {
        SimonSays.GetInstance().triggerPuzzle();
    }

}
