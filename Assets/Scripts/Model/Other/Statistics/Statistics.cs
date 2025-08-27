using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Statistics
{
    [SerializeField] private List<PlayerLevelStatistics> _data = new();
    [field: SerializeField] public string PlayerName { get; private set; }

    private PlayerLevelStatistics _levelStatisics;

    public int LevelsCount => _data.Count;

    public Statistics(string playerName)
    {
        PlayerName = playerName;
    }

    public void InitializeNewLevel(int level, int busesCount) =>
        _levelStatisics = new PlayerLevelStatistics(level, busesCount);

    public void IncrementPassengersArrange() =>
        _levelStatisics.IncrementPassengersArrange();

    public void IncrementBusShuffle() =>
        _levelStatisics.IncrementBusShuffle();

    public void CompleteLevel() =>
        _data.Add(_levelStatisics);

    public PlayerLevelStatistics GetPlayerDataAtIndex(int index) =>
        _data[index];
}
