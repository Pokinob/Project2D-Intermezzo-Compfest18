using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class claimSystem : MonoBehaviour, IDataPersistence
{
    [Header("Claim Trigger Settings")]
    [SerializeField]private bool playerInRange;

    [Header("Claim Settings")]
    [SerializeField] private itemID itemObj;
    [SerializeField] private string itemID;
    [SerializeField] private bool itemPuzzle;
    [SerializeField] private int levelItem;

    [ContextMenu("Generate ID")]
    public void GenerateID()
    {
        itemID = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        playerInRange = false;
        if(DataPersistenceManager.Instance != null)
        {
            if (DataPersistenceManager.GetInstance().gameData != null)
            {
                LoadData(DataPersistenceManager.GetInstance().gameData);
                return;
            }
        }
    }

    private void Update()
    {
        if (playerInRange)
        {
            if (DialogueManager.GetInstance().ClaimCoroutine != null)
            {
                StartCoroutine(disableItem());
                return;
            }
                DialogueManager.GetInstance().isClaim = itemID;
                DialogueManager.GetInstance().ItemName = itemObj.itemName;
                return;
        }
        else
        {
            if(DialogueManager.GetInstance().isClaim == itemID)
            {
                DialogueManager.GetInstance().isClaim = "";
                DialogueManager.GetInstance().ItemName = "";
                return;
            }
        }
    }

    IEnumerator disableItem()
    {

        DialogueManager manager = DialogueManager.GetInstance();
        yield return new WaitUntil(() => manager.ClaimCoroutine == null);

        if (itemPuzzle)
        {
            if (manager.dialogueVariables.variableDictionary.ContainsKey($"ItemP{levelItem}"))
            {
                int cnt = ((Ink.Runtime.IntValue)manager.dialogueVariables.variableDictionary[$"ItemP{levelItem}"]).value;
                cnt++;
                Ink.Runtime.IntValue temp = new Ink.Runtime.IntValue(cnt);
                manager.dialogueVariables.variableDictionary.Remove($"ItemP{levelItem}");
                manager.dialogueVariables.variableDictionary.Add($"ItemP{levelItem}", temp);
                inventoryManager.GetInstance().AddItem(itemID, 1);
            }

        }
        else
        {
            inventoryManager.GetInstance().AddItem(itemObj.itemName, 1);
        }
        DialogueManager.GetInstance().isClaim = "";
        DialogueManager.GetInstance().ItemName = "";
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void LoadData(GameData data)
    {
        if(data.itemData.ContainsKey(itemID))
        {
            if (!data.itemData[itemID])
            {
                gameObject.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                gameObject.transform.parent.gameObject.SetActive(true);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.itemData.ContainsKey(itemID))
        { 
            data.itemData.Remove(itemID);
        }
        data.itemData.Add(itemID, gameObject.transform.parent.gameObject.activeSelf);
    }
}
