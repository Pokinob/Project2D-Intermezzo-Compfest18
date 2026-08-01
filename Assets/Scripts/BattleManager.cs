using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    class BattleParticipant
    {
        public int health;
        public int maxHealth;

        public int attack;
        public int defense;
        public int accuracy;
        public int evade;

        public BattleParticipant(BattleStats data)
        {
            health = data.maxHealth;
            maxHealth = data.maxHealth;

            attack = data.attack;
            defense = data.defense;
            accuracy = data.accuracy;
            evade = data.evade;
        }
    }

    public BattleStats playerStats;
    public BattleStats[] enemyStats;

    string currentTurn = "Player";
    BattleParticipant player;
    List<BattleParticipant> enemies;

    void Start()
    {
        player = new BattleParticipant(playerStats);
        foreach (BattleStats enemyStat in enemyStats)
        {
            enemies.Add(new BattleParticipant(enemyStat));
        }
    }

    void Update()
    {
        if (player.health <= 0)
        {
            Debug.Log("player lose");
            return;
        }

        bool isEnemyAlive = false;
        foreach (BattleParticipant enemy in enemies)
        {
            if (enemy.health > 0)
            {
                isEnemyAlive = true;
                break;
            }
        }

        if (!isEnemyAlive)
        {
            Debug.Log("enemy lose");
            return;
        }
    }

    void PlayerAttack(BattleParticipant target, string move)
    {

    }
}
