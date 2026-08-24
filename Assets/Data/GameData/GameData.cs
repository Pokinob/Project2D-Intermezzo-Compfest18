using System.Collections.Generic;
using UnityEngine;

public enum typeItem
{
    heal,
    itemPuzzle,
    normal
}

[System.Serializable]
public class itemDatas
{
    public string itemName;
    public string itemGuid;
    public int itemCount;
    public typeItem typeItem;
    public int heal;
    public itemDatas(string itemName, typeItem typeItem, int heal)
    {
        this.itemName = itemName;
        this.typeItem = typeItem;
        this.itemCount = 1;
        this.heal = heal;
    }
}

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public SerializeDictionary<string, bool> itemData;
    public SerializeDictionary<string, itemDatas> inventory;
    public SerializeDictionary<int, bool> puzzleData;
    public string inkData;
    public GameData()
    {
        this.playerPosition = Vector3.zero;
        this.itemData = new SerializeDictionary<string, bool>();
        this.inventory = new SerializeDictionary<string, itemDatas>();
        this.puzzleData = new SerializeDictionary<int, bool>();
        this.inkData = null;
    }

}
