using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private static QuestManager instance;
    [SerializeField] private List<GameObject> objQuest1;

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
}
