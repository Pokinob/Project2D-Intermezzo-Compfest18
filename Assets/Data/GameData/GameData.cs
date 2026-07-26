using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public SerializeDictionary<string, bool> itemData;
    public SerializeDictionary<int, bool> puzzleData;
    public string inkData;

    public GameData()
    {
        this.playerPosition = Vector3.zero;
        this.itemData = new SerializeDictionary<string, bool>();
        this.puzzleData = new SerializeDictionary<int, bool>();
        this.inkData = null;
    }

}
