using System;
using UnityEngine;

public class LevelBusCalculator
{
    private readonly int _minLevel = 20;
    private readonly int _levelPeriod = 10;
    private readonly int _incrementPerLevel = 1;
    private readonly int _incrementPerLevelPeriod = 20;
    private readonly int _maxBusesInElevator = 20;
    private readonly int _level;

    public LevelBusCalculator(int level)
    {
        _level = level > 0 ? level : throw new ArgumentOutOfRangeException(nameof(level));
        Calculate(_level);
    }

    public int UndergroundBusesCount { get; private set; }

    public void Calculate(int level)
    {
        if (level < _minLevel)
            return;

        int levelPeriodCount = level / _levelPeriod - 1;
        int levelInPeriod = level % _levelPeriod;
        int busInPeriod = levelPeriodCount * _incrementPerLevelPeriod;
        int busInLevel = levelInPeriod * _incrementPerLevel;
        UndergroundBusesCount = busInPeriod + busInLevel;
    }

    public int GetElevatorsCount()
    {
        if (UndergroundBusesCount == 0)
            return 0;

        return UndergroundBusesCount / _maxBusesInElevator + 1;
    }

    public int GetSimpleLevel()
    {
        int simpleLevel = _level;
        int maxSimpleLevel = 19;

        if (simpleLevel > maxSimpleLevel)
            simpleLevel = simpleLevel % _levelPeriod + _levelPeriod;

        return simpleLevel;
    }
}
