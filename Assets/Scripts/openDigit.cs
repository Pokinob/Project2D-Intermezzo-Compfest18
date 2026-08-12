using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class openDigit : MonoBehaviour
{
    private bool inRangePlayer = false;
    [SerializeField] private GameObject digitPanel;
    [SerializeField] private TextMeshProUGUI inputFieldDigit;


    private void Update()
    {
        if(inRangePlayer && InputManager.GetInstance().GetInteractPressed() && !digitPanel.activeSelf)
        {
            inputFieldDigit.text = "";
            digitPanel.SetActive(true);
        }
    }

    public void closeDigitPanel()
    {
        if (inRangePlayer) {
            digitPanel.SetActive(false);
            inputFieldDigit.text = "";
        }
    }

    public void checkDigit()
    {
        string check = "";
        for (int i=1; i<=4; i++)
        {
            check += ((Ink.Runtime.IntValue)DialogueManager.GetInstance().dialogueVariables.variableDictionary[$"P4Digit{i}"]).value.ToString();
        }
        Debug.Log(check);
        if(inputFieldDigit.text == check)
        {
            Debug.Log("Correct Code");
        }
        else
        {
            StartCoroutine(resetInput());
        }
    }

    IEnumerator resetInput()
    {
        inputFieldDigit.text = "INVALID CODE";
        yield return new WaitForSeconds(1f);
        inputFieldDigit.text = "";
    }

    public void inputDigit(string digit)
    {
        if(inputFieldDigit.text.Length >= 4)
        {
            return;
        }
        inputFieldDigit.text += digit;
    }

    public void deleteDigit()
    {
        if (inputFieldDigit.text.Length ==0)
        {
            return;
        }
        inputFieldDigit.text.Remove(inputFieldDigit.text.Length - 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            inRangePlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRangePlayer = false;
        }
    }
}
