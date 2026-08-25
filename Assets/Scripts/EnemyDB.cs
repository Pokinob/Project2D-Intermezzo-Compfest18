using Ink.Parsed;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDB : MonoBehaviour
{

    public List<BattleStats> enemyDataArray;
    public static EnemyDB instance;
    public static EnemyDB GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }
}
