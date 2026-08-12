using UnityEngine;

public class Testtrigger : MonoBehaviour
{
    bool startP4 = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !startP4)
        {
            QuestManager.GetInstance().startP4();
            startP4 = true;
        }
    }
}
