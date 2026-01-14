using System;
using UnityEngine;

namespace Scripts.Model.Other.StatisticsData
{
    [Serializable]
    public struct PlayerLevelStatistics
    {
        public PlayerLevelStatistics(int level, int busesCount)
        {
            Level = level;
            BusesCount = busesCount;
            PassengersArrangeCount = 0;
            BusShuffleCount = 0;
        }

        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public int PassengersArrangeCount { get; private set; }
        [field: SerializeField] public int BusShuffleCount { get; private set; }
        [field: SerializeField] public int BusesCount { get; private set; }

        public void IncrementPassengersArrange() =>
            PassengersArrangeCount++;

        public void IncrementBusShuffle() =>
            BusShuffleCount++;
    }
}