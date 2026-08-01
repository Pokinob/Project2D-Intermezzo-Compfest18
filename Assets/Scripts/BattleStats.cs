using UnityEngine;

[CreateAssetMenu(fileName = "BattleStats", menuName = "Scriptable Objects/BattleStats")]
public class BattleStats : ScriptableObject
{
    public int maxHealth;
    public int attack;
    public int defense;
    public int accuracy;
    public int evade;
}
