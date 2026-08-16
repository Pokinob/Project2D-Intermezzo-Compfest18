using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerBattle : MonoBehaviour
{
    [System.Serializable]
    public class BattleCount
    {
        public BattleStats[] enemyStats;
        public List<GameObject> enemyPosition;
        public BattleCount(BattleCount other)
        {
            enemyStats = other.enemyStats;
            enemyPosition = other.enemyPosition;
        }
    }

    private bool isTriggered = false;
    private Coroutine coroutine;
    [SerializeField] private int tryRng;
    [SerializeField] private int minRng;
    [SerializeField] private int maxRng;
    [SerializeField] private GameObject playerPos;
    [SerializeField] private List<BattleCount> battleCounts = new List<BattleCount>();

    private void Update()
    {
        if (isTriggered && InputManager.GetInstance().GetMoveDirection() != Vector2.zero && coroutine == null && !BattleManager.GetInstance().isBattleActive)
        {
            coroutine = StartCoroutine(checkRng());
        }
    }

    IEnumerator checkRng()
    {
        int rng = Random.Range(minRng, maxRng);
        if (rng > tryRng)
        {
            Debug.Log($"Starting battle! {rng}");
            int randomCount = Random.Range(0, 2);
            BattleManager.GetInstance().StartBattle(battleCounts[randomCount].enemyStats, battleCounts[randomCount].enemyPosition, playerPos);
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
