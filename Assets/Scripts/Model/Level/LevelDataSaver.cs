using UnityEngine;
using System.IO;

public static class LevelDataSaver
{
    public static void Save(LevelsDataContainer currentLevelsData)
    {
        string localSavePath = "F:/GitProjects/PassengerTransportation/Assets/Resources";

        string json = JsonUtility.ToJson(currentLevelsData, true);

        string fileName = "Levels";
        string path = $"{localSavePath}/{fileName}.json";

        File.WriteAllText(path, json);

        Debug.Log("Save Complete!!!");
    }
}
