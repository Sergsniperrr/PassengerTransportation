using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class StatisticsSaver
{
    public static void Save(Statistics statistics)
    {
        string localSavePath = "F:/GitProjects/PassengerTransportation/Assets/Resources";

        string json = JsonUtility.ToJson(statistics, true);

        string fileName = statistics.PlayerName;
        string path = $"{localSavePath}/{fileName}.json";

        File.WriteAllText(path, json);

        Debug.Log("Save Complete!!!");
    }

    public static void ExportStatistics(List<string> dataList)
    {
        string filePath = "C:/Users/Sergsniper/Downloads/GameStats/StatisticsOnSpendingGameMoney.txt";

        using (StreamWriter writer = new(filePath))
        {
            foreach (string item in dataList)
            {
                writer.WriteLine(item);
            }
        }

        Debug.Log("The data has been successfully saved to a file!");
    }
}
