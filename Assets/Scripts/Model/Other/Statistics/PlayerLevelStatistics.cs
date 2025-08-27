using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerLevelStatistics
{
    [field: SerializeField] public int Level { get; private set; }
    [field: SerializeField] public int PassengersArrangeCount { get; private set; }
    [field: SerializeField] public int BusShuffleCount { get; private set; }
    [field: SerializeField] public int BusesCount { get; private set; }
    [field: SerializeField] public TimeSpan Duration { get; private set; }

    private DateTime _beginTime;

    public PlayerLevelStatistics(int level, int busesCount)
    {
        Level = level;
        BusesCount = busesCount;
        PassengersArrangeCount = 0;
        BusShuffleCount = 0;
        _beginTime = DateTime.Now;
        Duration = TimeSpan.FromSeconds(0);
    }

    public void IncrementPassengersArrange() =>
        PassengersArrangeCount++;

    public void IncrementBusShuffle() =>
        BusShuffleCount++;

    public void StopTimer()
    {
        Duration = DateTime.Now - _beginTime;
    }
}
