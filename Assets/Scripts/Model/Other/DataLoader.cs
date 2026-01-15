using Scripts.Model.Levels;
using UnityEngine;

namespace Scripts.Model.Other
{
    public static class DataLoader
    {
        public static LevelsDataContainer GetLevelData(TextAsset jsonResource) =>
            JsonUtility.FromJson<LevelsDataContainer>(jsonResource.text);
    }
}