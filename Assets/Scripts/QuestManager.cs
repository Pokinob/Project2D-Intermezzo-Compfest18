using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    [SerializeField] private List<GameObject> objQuest1;
    [SerializeField] private List<GameObject> objQuest2;
    [SerializeField] private TextAsset inkFilesP2;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of QuestManager found!");
            return;
        }
        instance = this;
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
        //Debug.Log(DialogueManager.GetInstance().dialogueVariables.variableDictionary["P2Complete"]);
        
        if((bool)DialogueManager.GetInstance().dialogueVariables.variableDictionary["P2Complete"] == true)
        {
            Debug.Log("P2 already completed, not starting dialogue");
            return;
        }
        int count = 0;
        foreach (GameObject obj in objQuest2)
        {
            if (obj.GetComponent<triggerBoulder>().isBoulderTriggered)
            {
                count++;
            }
        }
        if(count == objQuest2.Count)
        {
            Debug.Log("All boulders triggered, starting dialogue for P2");
            DialogueManager.GetInstance().EnterDialogueMode(inkFilesP2);
        }
    }

}
