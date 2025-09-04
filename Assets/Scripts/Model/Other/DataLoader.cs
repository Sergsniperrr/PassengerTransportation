using UnityEngine;

public static class DataLoader
{
    private const string LevelPrefName = "CurrentLevel";
    private const string MoneyPrefName = "Money";
    private const string ScorePrefName = "Score";

    public static int CurrentLevel => PlayerPrefs.GetInt(LevelPrefName, 1);
    public static int Money => PlayerPrefs.GetInt(MoneyPrefName, 0);
    public static int Score => PlayerPrefs.GetInt(ScorePrefName, 0);

    public static LevelsDataContainer GetLevelData(TextAsset jsonResource) =>
        JsonUtility.FromJson<LevelsDataContainer>(jsonResource.text);
}
