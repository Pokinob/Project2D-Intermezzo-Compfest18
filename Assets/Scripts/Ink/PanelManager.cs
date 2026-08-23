using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject savePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private TextAsset loadglobalsInkFile;

    [SerializeField] private DialogueManager dialogueManager;
    private static PanelManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Multiple instances of PanelManager found! Destroying duplicate.");
            return;
        }
        instance = this;
    }

    public void OpenLoadPanel()
    {
        loadPanel.SetActive(true);
        mainPanel.SetActive(false);
    }


    public void closeLoadPanel()
    {
        loadPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void StartLoad()
    {
        mainPanel.SetActive(false);
        loadPanel.SetActive(false);
    }

    public void OpenSavePanel()
    {
        savePanel.SetActive(true);
    }

    public void CloseSavePanel()
    {
        savePanel.SetActive(false);
    }


    public void NewGamePanel()
    {
        mainPanel.SetActive(false);
        loadPanel.SetActive(false);
        savePanel.SetActive(false);
        dialogueManager.startGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenSettingsPanel()
    {
        Debug.Log("Opening settings panel");
        return;
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettingsPanel()
    {
        Debug.Log("Closing settings panel");
        return;
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

}
