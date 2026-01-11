using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Level
{
    [Serializable]
    public class LevelsDataContainer
    {
        [SerializeField] private List<LevelData> _levels = new();

        public int Count => _levels.Count;

        public LevelData GetLevel(int levelNumber)
        {
            if (_levels.Count == 0)
                throw new Exception("LEVELS COUNT = 0!");

            if (levelNumber >= _levels.Count && levelNumber < 0)
                throw new ArgumentOutOfRangeException(nameof(levelNumber));

            return _levels[levelNumber - 1];
        }

        public void AddLevel(LevelData level)
        {
            List<LevelData> levels = new(_levels);

            levels.Add(level);
            _levels = levels;
        }

        public void ReplaceLevel(LevelData level, int number)
        {
            if (number >= _levels.Count && number < 0)
                throw new ArgumentOutOfRangeException(nameof(number));

            _levels[number] = level;
        }
    }
}
