using UnityEngine;
using System;
using System.IO;

public class DataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public DataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load(int saveSlot)
    {
        string fullPath = Path.Combine(dataDirPath, $"{dataFileName}_{saveSlot}");
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                string dataFromFile = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataFromFile = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataFromFile);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occurred when trying to load data from file: " + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data, int saveSlot)
    {
        string fullPath = Path.Combine(dataDirPath, $"{dataFileName}_{saveSlot}");
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }
}

