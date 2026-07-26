using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour, IDataPersistence
{
    [Header("Dialogue Trigger Settings")]
    private bool playerInRange;

    [Header("Ink Asset")]
    [SerializeField] private TextAsset inkJSON;

    [Header("Claim Settings")]
    [SerializeField] private GameObject claimObject;
    [SerializeField] private string isClaim;

    [ContextMenu("Generate Unique IDitem")]
    private void GenerateUniqueID()
    {
        isClaim = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && DialogueManager.GetInstance().isClaim && claimObject != null)
        {
            claimObject.SetActive(false);
        }
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            if(InputManager.GetInstance().GetInteractPressed())
            {
                //Debug.Log("Dialogue Triggered");
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            }
        }
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
        if(collider.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void LoadData(GameData data)
    {
        if(data != null)
        {
            if(data.itemData.ContainsKey(isClaim))
            {
                 if(data.itemData[isClaim] == false)
                 {
                      claimObject.SetActive(false);
                 }
            }
        }
        
    }

    public void SaveData(ref GameData data)
    {
        if(claimObject != null)
        {
            if (data.itemData.ContainsKey(isClaim))
            {
                data.itemData.Remove(isClaim);
            }
            data.itemData.Add(isClaim, claimObject.activeSelf);
        }       
    }
}
