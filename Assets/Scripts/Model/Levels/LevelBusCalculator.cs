using System;

namespace Scripts.Model.Levels
{
    public class LevelBusCalculator
    {
        private const int HalfDivider = 2;
        private const int MinLevel = 20;
        private const int LevelPeriod = 10;
        private const int IncrementPerLevel = 1;
        private const int IncrementPerLevelPeriod = 20;
        private const int MaxBusesInElevator = 20;
        private const int MaxSimpleLevel = 19;

        private readonly int _level;

        public LevelBusCalculator(int level)
        {
            _level = level > 0 ? level : throw new ArgumentOutOfRangeException(nameof(level));
            Calculate(_level);
        }

        public int UndergroundBusesCount { get; private set; }

        private void Calculate(int level)
        {
            if (level < MinLevel)
                return;

            int levelPeriodCount = level / LevelPeriod - 1;
            int levelInPeriod = level % LevelPeriod;
            int busInPeriod = levelPeriodCount * IncrementPerLevelPeriod - (IncrementPerLevelPeriod / HalfDivider);
            int busInLevel = levelInPeriod * IncrementPerLevel;
            UndergroundBusesCount = busInPeriod + busInLevel;
        }

        public int GetElevatorsCount()
        {
            if (UndergroundBusesCount == 0)
                return 0;

            return UndergroundBusesCount / MaxBusesInElevator + 1;
        }

        public int GetSimpleLevel()
        {
            int simpleLevel = _level;

            if (simpleLevel > MaxSimpleLevel)
                simpleLevel = simpleLevel % LevelPeriod + LevelPeriod;

            return simpleLevel;
        }
    }
}