using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsCollector : MonoBehaviour
{
    [SerializeField] private string _playerName;
    [SerializeField] private Level _level;
    [SerializeField] private PassengerColorArranger _passengerArranger;
    [SerializeField] private PassengerColorArranger _viewingAds;
    [SerializeField] private BusColorsShuffler _busShuffler;

    private Statistics _statistics;
    private int _maxLevel = 40;

    private void Awake()
    {
        _statistics = new(_playerName);
    }

    private void OnEnable()
    {
        _level.BussesCountInitialized += InitialNewLevel;
        _passengerArranger.PassengersArranged += _statistics.IncrementPassengersArrange;
        _viewingAds.PassengersArranged += _statistics.IncrementPassengersArrange;
        _busShuffler.BusesShuffled += _statistics.IncrementBusShuffle;
        _level.Completed += WriteStatistics;
    }

    private void FinishCollectingStatistics()
    {
        _level.BussesCountInitialized -= InitialNewLevel;
        _passengerArranger.PassengersArranged -= _statistics.IncrementPassengersArrange;
        _viewingAds.PassengersArranged -= _statistics.IncrementPassengersArrange;
        _busShuffler.BusesShuffled -= _statistics.IncrementBusShuffle;
        _level.Completed -= WriteStatistics;
    }

    private void InitialNewLevel(int busesCount)
    {
        if (_level.CurrentLevel > _maxLevel + 1)
            return;

        if (_level.CurrentLevel == _maxLevel + 1)
        {
            FinishCollectingStatistics();
            StatisticsSaver.Save(_statistics);
            return;
        }

        _statistics.InitializeNewLevel(_level.CurrentLevel, busesCount);

        Debug.Log($"Level {_level.CurrentLevel} been initialized!!!");
    }

    private void WriteStatistics()
    {
        if (_level.CurrentLevel > _maxLevel)
            return;

        _statistics.CompleteLevel();

        Debug.Log($"Level {_level.CurrentLevel} statistics added!!!");
    }
}
