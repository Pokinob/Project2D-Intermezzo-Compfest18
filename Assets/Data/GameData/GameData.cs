using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public SerializeDictionary<string, bool> itemData;
    public string inkData;

    public GameData()
    {
        this.playerPosition = Vector3.zero;
        this.itemData = new SerializeDictionary<string, bool>();
        this.inkData = null;
    }

}
