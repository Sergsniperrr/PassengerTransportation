using System.IO;
using UnityEngine;

namespace Scripts.Model.Levels
{
    public static class LevelDataSaver
    {
        public static void Save(LevelsDataContainer currentLevelsData)
        {
            const string LocalSavePath = "F:/GitProjects/PassengerTransportation/Assets/Resources";

            string json = JsonUtility.ToJson(currentLevelsData, true);

            string fileName = "Levels";
            string path = $"{LocalSavePath}/{fileName}.json";

            File.WriteAllText(path, json);

            Debug.Log("Save Complete!!!");
        }
    }
}