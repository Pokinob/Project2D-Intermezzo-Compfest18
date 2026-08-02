using UnityEngine;

public class triggerBoulder : MonoBehaviour
{
    public bool isBoulderTriggered {  get; private set; }

    private void Awake()
    {
        isBoulderTriggered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PushObject"))
        {
            isBoulderTriggered = true;
            QuestManager.GetInstance().completedPuzzle2();
            Debug.Log("Boulder triggered");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("PushObject"))
        {
            isBoulderTriggered = false;
        }
    }
}
