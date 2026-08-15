using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class skills
{
    public SkillType type;
    public string name;
    public int damage;
    public int heal;
    public int accuracy;
    public int priority;
    public int cooldown;
    public int cooldownRemaining;
    public skills(){
        cooldownRemaining = 0;
    }

    public skills(skills other)
    {
        type = other.type;
        name = other.name;
        damage = other.damage;
        heal = other.heal;
        accuracy = other.accuracy;
        priority = other.priority;
        cooldown = other.cooldown;
        cooldownRemaining = 0;
    }

}

public enum SkillType
{
    Attack,
    Heal,
    Evade,
}



[CreateAssetMenu(fileName = "BattleStats", menuName = "Scriptable Objects/BattleStats")]
public class BattleStats : ScriptableObject
{
    public GameObject prefab;
    public string nameChar;
    public int maxHealth;
    public int defense;
    public int evade;
    public int speed;
    public List<skills> Skills;
}
