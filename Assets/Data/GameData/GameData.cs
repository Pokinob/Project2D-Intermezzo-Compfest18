using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    //public int healItem;
    public string inkData;

    public GameData()
    {
        this.playerPosition = Vector3.zero;
        //this.healItem = 0;
        this.inkData = null;
    }

}
