using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "itemDB", menuName = "Scriptable Objects/itemDB")]
public class itemDB : ScriptableObject
{
    public Dictionary<string, ScriptableObject> itemDictionary = new Dictionary<string, ScriptableObject>();

    public itemDB(Dictionary<string, ScriptableObject> items){
        this.itemDictionary = items;
    }

}
