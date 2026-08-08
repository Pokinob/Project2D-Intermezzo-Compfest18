using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class P3Manager : MonoBehaviour
{
    private static P3Manager instance;
    [SerializeField] private string Pattern;
    [SerializeField] private string hiddenPattern;
    [SerializeField] List<char> patternList;
    [SerializeField] List<char> hiddenPatternList;
    [SerializeField] private PlayableDirector entryCutscene;
    [SerializeField] private PlayableDirector hiddenCutscene;
    [SerializeField] private PlayableDirector afterCutscene;
    [SerializeField] private PlayableDirector exitPuzzle;
    [SerializeField] private PlayableDirector fadeEntry;
    private bool hiddenActive = false;
    private bool normalActive = false;
    private bool puzzleCompleted = false;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of P3Manager found!");
            return;
        }
        instance = this;
    }

    public static P3Manager GetInstance()
    {
        return instance;
    }

    public IEnumerator resetPuzzle()
    {
        fadeEntry.Play();
        yield return new WaitForSeconds(1f);
        hiddenActive = false;
        normalActive = false;
        patternList = new List<char>(Pattern.ToCharArray());
        if (DialogueManager.GetInstance().dialogueVariables.variableDictionary["claimP3"])
        {
            hiddenPatternList = new List<char>(hiddenPattern.ToCharArray());
        }
        entryCutscene.Play();
    }

    public IEnumerator checkEntry(int entryNumber)
    {
        fadeEntry.Play();
        yield return new WaitForSeconds(1f);
        if (puzzleCompleted)
        {
            exitPuzzle.Play();
            yield return null;
        }
        char expectedEntry='\0';
        char expectedHiddenEntry='\0';
        if (patternList.Count > 0)
        {
            expectedEntry = patternList[0];
        }
        if (hiddenPatternList.Count > 0)
        {
            expectedHiddenEntry = hiddenPatternList[0];
        }
        if (patternList.Count > 0)
        {
            if (expectedEntry == entryNumber.ToString()[0] && !hiddenActive)
            {
                normalActive = true;
                Debug.Log("Correct entry: " + entryNumber);
                patternList.RemoveAt(0);
                if (patternList.Count == 0)
                {
                    Debug.Log("Pattern completed!");
                    afterCutscene.Play();
                    yield return null;
                }
                else
                {
                    entryCutscene.Play();
                }
                
            }else if (!hiddenActive && normalActive)
            {
                exitPuzzle.Play();
                yield return null;
            }
        if (hiddenPatternList.Count>0)
        {
            if(expectedHiddenEntry == entryNumber.ToString()[0] && !normalActive)
            {
                hiddenActive = true;
                Debug.Log("Correct hidden entry: " + entryNumber);
                hiddenPatternList.RemoveAt(0);
                if (hiddenPatternList.Count == 0)
                {
                    Debug.Log("Hidden pattern completed!");
                    hiddenCutscene.Play();
                    yield return null;
                } else
                {
                    entryCutscene.Play();
                    yield return null;
                    }
            } else if (!normalActive && hiddenActive) {
                    exitPuzzle.Play();
                    yield return null;
                }
            }
        }
        if (!hiddenActive && !normalActive)
        {
            exitPuzzle.Play();
            yield return null;
        }
    }


}
