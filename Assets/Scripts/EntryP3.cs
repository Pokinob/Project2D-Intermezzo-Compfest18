using UnityEngine;

public class EntryP3 : MonoBehaviour
{
    [SerializeField] private int entryNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !PlayerOverworld.GetInstance().isFreeze)
        {
            PlayerOverworld.GetInstance().StartTimeline();
            if (entryNumber != 0)
            {
                StartCoroutine(P3Manager.GetInstance().checkEntry(entryNumber));
            }
            else
            {
                StartCoroutine(P3Manager.GetInstance().resetPuzzle());
            }
        }
    }
}
