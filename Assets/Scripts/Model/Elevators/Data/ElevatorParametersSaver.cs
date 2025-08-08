using UnityEngine;
using System.IO;

public static class ElevatorParametersSaver
{
    public static void Save(ElevatorsDataContainer currentLevelsData)
    {
        string localSavePath = "F:/GitProjects/PassengerTransportation/Assets/Resources";

        string json = JsonUtility.ToJson(currentLevelsData, true);

        string fileName = "ElevatorsParameters";
        string path = $"{localSavePath}/{fileName}.json";

        File.WriteAllText(path, json);

        Debug.Log("Save Complete!!!");
    }
}
