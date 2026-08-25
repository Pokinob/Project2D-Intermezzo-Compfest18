using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class inventoryManager : MonoBehaviour, IDataPersistence
{
    public List<itemID> itemDB;
    public Dictionary<string, itemID> itemDictionary;
    public Dictionary<string, itemDatas> inventory;
    public static inventoryManager instance;

    public static inventoryManager GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        inventory = new Dictionary<string, itemDatas>();
        itemDictionary = new Dictionary<string, itemID>();
        foreach (var item in itemDB)
        {
            itemDictionary.Add(item.itemName, item);
        }
    }

    public void AddItem(string itemName, int count)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName].itemCount += count;
        }
        else
        {
            if (itemDictionary.ContainsKey(itemName))
            inventory.Add(itemName, new itemDatas(itemName, itemDictionary[itemName].typeItem, itemDictionary[itemName].heal));
            else
            {
                inventory.Add(itemName, new itemDatas(itemName, typeItem.normal, 0));
            }
            inventory[itemName].itemCount += count-1;
        }
    }

    public void RemoveItem(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName].itemCount--;
            if (inventory[itemName].itemCount <= 0)
            {
                inventory.Remove(itemName);
            }
        }
    }
    public void LoadData(GameData data)
    {
        inventory.Clear();
        foreach (var item in data.inventory)
        {
            inventory.Add(item.Key, item.Value);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.inventory.Clear();
        foreach (var item in inventory)
        {
            Debug.Log(item.Key);
            data.inventory.Add(item.Key, item.Value);
        }
    }

    public void addhealtest()
    {
        AddItem(itemDictionary["Potion"].itemName, 1);
    }
}
