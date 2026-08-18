using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanelManager : MonoBehaviour
{
    [SerializeField] private GameObject loadPanel;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject savePanel;
    [SerializeField] private TextAsset loadglobalsInkFile;

    [SerializeField] private DialogueManager dialogueManager;
    private static PanelManager instance;

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

}
