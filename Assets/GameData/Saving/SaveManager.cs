using UnityEngine;
using System.IO;

public static class SaveManager
{
    public static string directory = "/SaveData/";
    public static string fileName = "MyData.txt";

    public static void Save(DataValues dv)
    {
        string dir = Application.persistentDataPath + directory;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        string json = JsonUtility.ToJson(dv);
        File.WriteAllText(dir + fileName, json);
        Debug.Log("Located here: " + (dir + fileName));
    }

    public static DataValues Load()
    {
        string fullPath = Application.persistentDataPath + directory + fileName;
        DataValues dv = new DataValues();

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            dv = JsonUtility.FromJson<DataValues>(json);
        }
        else
        {
            Debug.Log("Save file does not exist");
        }

        return dv;
    }
}
