using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DialogueVariable : IDataPersistence
{
    public Dictionary<string, Ink.Runtime.Object> variableDictionary { get; private set; }
    private Story globalVariable;

    public DialogueVariable(TextAsset globalsInkFile, string jsonState = null)
    {
        globalVariable = new Story(globalsInkFile.text);

        if(jsonState != null)
        {
            globalVariable.state.LoadJson(jsonState);
        }

        variableDictionary = new Dictionary<string, Ink.Runtime.Object>();

        foreach (string name in globalVariable.variablesState)
        {
            Ink.Runtime.Object value = globalVariable.variablesState.GetVariableWithName(name);
            variableDictionary.Add(name, value);
            Debug.Log($"Variable '{name}' initialized with value: {value}");
        }
    }

    public void StartListening(Story story)
    {
        VariableToStory(story);
        story.variablesState.variableChangedEvent += variableChange;
    }

    public void StopListening(Story story)
    {
        // Unsubscribe from the variable change event
        story.variablesState.variableChangedEvent -= variableChange;
    }

    private void variableChange(string variableName, Ink.Runtime.Object value)
    {
        // Handle the variable change here
        Debug.Log($"Variable '{variableName}' changed to: {value}");
        if (variableDictionary.ContainsKey(variableName))
        {
            variableDictionary.Remove(variableName);
            variableDictionary.Add(variableName, value);
        }
    }  

    private void VariableToStory(Story story)
    {
        foreach (var kvp in variableDictionary)
        {
            story.variablesState.SetGlobal(kvp.Key, kvp.Value);
        }
    }

    public void LoadData(GameData data)
    {
        //nothing to load, as the global variables are already loaded in the dialoguemanager
    }

    public void SaveData(ref GameData data)
    {
        VariableToStory(globalVariable);
        data.inkData = globalVariable.state.ToJson();
        Debug.Log($"Saving data: {data.inkData}");
    }
}
