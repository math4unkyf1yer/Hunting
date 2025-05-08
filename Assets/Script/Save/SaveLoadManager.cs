using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private string savePath;
    public static SaveLoadManager Intance;

    private void Awake()
    {
        Intance = this;
        savePath = Application.persistentDataPath + "/gamedata.save";
    }

    public void SaveGame(GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(savePath, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }
    }

    public GameData LoadGame()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(savePath, FileMode.Open))
            {
                return formatter.Deserialize(stream) as GameData;
            }
        }
        else
        {
            Debug.LogWarning("Save file not found!");
            return null;
        }
    }
    public bool SaveExists()
    {
        return File.Exists(savePath);
    }

}
