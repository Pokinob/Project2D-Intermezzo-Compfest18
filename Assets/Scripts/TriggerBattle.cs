using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerBattle : MonoBehaviour
{
    private bool isTriggered = false;
    private Coroutine coroutine;
    [SerializeField] private int tryRng;
    [SerializeField] private int minRng;
    [SerializeField] private int maxRng;
    [SerializeField] private bool isBattleActive = false;
    [SerializeField] private BattleStats[] enemyStats;
    [SerializeField] private GameObject playerPos;
    [SerializeField] private List<GameObject> enemyPosition;

    private void Update()
    {
        if (isTriggered && InputManager.GetInstance().GetMoveDirection() != Vector2.zero && coroutine == null && !isBattleActive)
        {
            coroutine = StartCoroutine(checkRng());
        }
    }

    IEnumerator checkRng()
    {
        int rng = Random.Range(minRng, maxRng);
        if (rng > tryRng)
        {
            isBattleActive = true;
            Debug.Log($"Starting battle! {rng}");
            BattleManager.GetInstance().StartBattle(enemyStats, enemyPosition, playerPos);
            PlayerOverworld.GetInstance().isFreeze = true;
            minRng = 0;
        }
        else
        {
            minRng += 10;
        }
        yield return new WaitForSeconds(1f);
        coroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTriggered = false;
            minRng = 0;
            coroutine = null;
        }
    }
}
