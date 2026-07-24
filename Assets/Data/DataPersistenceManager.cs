using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Unity.VisualScripting;


public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private DataHandler handler;
    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Found more than one DataPersistenceManager in the scene.");
        }
        Instance = this;
    }

    private void Start()
    {
        this.handler = new DataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void SaveGame(int saveSlot)
    {
        Debug.Log("Saving game to slot " + saveSlot);
        foreach (var item in dataPersistenceObjects)
        {
            item.SaveData(ref gameData);
        }

        handler.Save(gameData, saveSlot);
    }

    public void LoadGame(int saveSlot)
    {
        this.gameData = handler.Load(saveSlot);

        if (this.gameData == null)
        {
            Debug.LogWarning("No data found. Initializing data to defaults.");
            NewGame();
        }

        foreach (IDataPersistence item in dataPersistenceObjects)
        {
            Debug.Log("Loaded data for " + item.GetType().Name);
            item.LoadData(gameData);
        }
        //Debug.Log("Game loaded from slot " + saveSlot);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

}
