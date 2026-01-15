using System.IO;
using UnityEngine;

namespace Scripts.Model.Levels
{
    public static class LevelDataSaver
    {
        public static void Save(LevelsDataContainer currentLevelsData)
        {
            const string LocalSavePath = "F:/GitProjects/PassengerTransportation/Assets/Resources";
            const string FileName = "Levels";

            string json = JsonUtility.ToJson(currentLevelsData, true);
            string path = $"{LocalSavePath}/{FileName}.json";

            File.WriteAllText(path, json);
        }
    }
}