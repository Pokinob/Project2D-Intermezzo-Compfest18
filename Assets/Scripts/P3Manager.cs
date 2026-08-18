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
    [SerializeField] private GameObject startPos;
    [SerializeField] private GameObject finishPos;
    [SerializeField] private GameObject hiddenPos;
    private bool hiddenActive = false;
    private bool normalActive = false;
    private bool puzzleCompleted = false;
    private bool hiddenClaim = false;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of P3Manager found! Destroying duplicate.");
            return;
        }
        instance = this;
    }

    public static P3Manager GetInstance()
    {
        return instance;
    }

    public GameObject resetPuzzle()
    {
        if (puzzleCompleted)
        {
            return startPos;
        }
        hiddenActive = false;
        normalActive = false;
        patternList = new List<char>(Pattern.ToCharArray());
        if (!((Ink.Runtime.BoolValue)DialogueManager.GetInstance().dialogueVariables.variableDictionary["claimP3"]).value)
        {
            hiddenPatternList = new List<char>(hiddenPattern.ToCharArray());
        }
        return null;
    }

    public GameObject checkEntry(int entryNumber)
    {
        if (puzzleCompleted)
        {
            return startPos;
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
        //cek normalPattern dulu, kalau ndak ada ke hiddenPattern
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
                    puzzleCompleted = true;
                    return finishPos;
                }
                else
                {
                    return null;
                }
                
            }else if (!hiddenActive && normalActive)
            {
                return startPos;
            }
        }

        //Kalau bukan normalPattern, cek hiddenPattern
        if (hiddenPatternList.Count > 0)
        {
            if (expectedHiddenEntry == entryNumber.ToString()[0] && !normalActive)
            {
                hiddenActive = true;
                Debug.Log("Correct hidden entry: " + entryNumber);

                hiddenPatternList.RemoveAt(0);

                if (hiddenPatternList.Count == 0)
                {
                    Debug.Log("Hidden pattern completed!");
                    hiddenClaim = true;
                    Ink.Runtime.Object variable = new Ink.Runtime.BoolValue(true);
                    DialogueManager.GetInstance().dialogueVariables.variableDictionary.Remove("claimP3");
                    DialogueManager.GetInstance().dialogueVariables.variableDictionary.Add("claimP3", variable);
                    return hiddenPos;
                }
                else
                {
                    return null;
                }
            }
            else if (!normalActive && hiddenActive)
            {
                return startPos;
            }
        }

        //Kalau bukan normalPattern dan hiddenPattern, return ke startPos
        if (!hiddenActive && !normalActive)
        {
            return startPos;
        }
        if (hiddenClaim)
        {
            return startPos;
        }
        //ini buat safety aja, kalau ga return ntar error
        return null;
    }

}
