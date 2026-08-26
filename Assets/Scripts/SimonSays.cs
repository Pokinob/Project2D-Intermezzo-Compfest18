using Ink.Parsed;
using Ink.Runtime;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.Playables;
using Ink.UnityIntegration;

public class SimonSays : MonoBehaviour
{
    private List<int> sequence;
    private int maxSimon;
    private int startIndex = 0;
    private int checkIndex = 0;
    private bool canClick = false;
    [SerializeField] private InkFile completeAsset;
    [SerializeField] private List<GameObject> objSequence;
    [SerializeField] private PlayableDirector playCutscene;
    [SerializeField] private PlayableDirector afterCutscene;
    public static SimonSays instance;

    public static SimonSays GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of SimonSays found!");
            return;
        }
        instance = this;
        maxSimon = 5;
    }

    public void triggerPuzzle()
    {
        sequence = new List<int>();
        startIndex = 0;
        checkIndex = 0;
        canClick = false;
        continuePuzzle();
    }

    private void continuePuzzle()
    {
        startIndex++;
        if (startIndex > maxSimon)
        {
            DialogueManager.GetInstance().EnterDialogueMode(completeAsset);
            return;
        }
        DialogueManager.GetInstance().canContinue = false;
        canClick = false;
        sequence.Add(Random.Range(1, 4));
        playCutscene.Play();
    }

    public void startShow()
    {
        StartCoroutine(showSequence());
    }

    public void checkBtn(int buttonIndex)
    {
        if (!canClick || startIndex > maxSimon || startIndex == 0) return;
        if (sequence[checkIndex] == buttonIndex)
        {
            Debug.Log($"Correct button {buttonIndex}");
            checkIndex++;
            if (checkIndex >= sequence.Count)
            {
                Debug.Log("Sequence completed successfully!");
                continuePuzzle();
            } 
        }
        else
        {
            Debug.Log($"Incorrect button {buttonIndex}");
            resetPuzzle();
        }
    }

    IEnumerator showSequence()
    {
        for (int i = 0; i < sequence.Count; i++)
        {
            int buttonIndex = sequence[i];
            objSequence[buttonIndex - 1].SetActive(true);
            yield return new WaitForSeconds(2f);
            objSequence[buttonIndex - 1].SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        canClick = true;
        checkIndex = 0;
        DialogueManager.GetInstance().canContinue = true;
        playCutscene.Stop();
        afterCutscene.Play();
    }

    // reset puzzle or if the player fails, reset the puzzle
    private void resetPuzzle()
    {
        triggerPuzzle();
    }
}
