using UnityEngine;

public static class DataSaver
{
    private const string LevelPrefName = "CurrentLevel";
    private const string MoneyPrefName = "Money";
    private const string ScorePrefName = "Score";

    public static void SaveLevelData(int level, int money, int score)
    {
        PlayerPrefs.SetInt(LevelPrefName, level);
        PlayerPrefs.SetInt(MoneyPrefName, money);
        PlayerPrefs.SetInt(ScorePrefName, score);
    }
}
