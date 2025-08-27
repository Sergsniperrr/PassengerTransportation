using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsCalculator : MonoBehaviour
{
    [SerializeField] private TextAsset[] _data;

    private readonly int _priceOfPassengersArrange = 20;
    private readonly int _priceOfBusShuffle = 4;

    private int _levelsCount;
    private List<Statistics> _allStatistics = new();

    private List<float> _averageExpensesAtLevels;
    private List<float> _averageExpensesAtLevelsForBus;
    private List<float> _minExpensesAtLevels;
    private List<float> _maxExpensesAtLevels;
    private List<float> _minExpensesAtLevelsForBus;
    private List<float> _maxExpensesAtLevelsForBus;

    private void OnDisable()
    {
        CalculateAverageExpenses();
        ExportStatistics();
    }

    private void CalculateAverageExpenses()
    {
        foreach (TextAsset json in _data)
            _allStatistics.Add(JsonUtility.FromJson<Statistics>(json.text));

        _averageExpensesAtLevels = new List<float>();
        _averageExpensesAtLevelsForBus = new List<float>();
        _minExpensesAtLevels = new List<float>();
        _maxExpensesAtLevels = new List<float>();
        _minExpensesAtLevelsForBus = new List<float>();
        _maxExpensesAtLevelsForBus = new List<float>();

        List<int> values = new();
        PlayerLevelStatistics player;
        _levelsCount = _allStatistics[0].LevelsCount;
        int busesCount;
        int expenses;
        int personalExpenses;
        float averageExpenses;
        float minValue;
        float maxValue;

        for (int i = 0; i < _levelsCount; i++)
        {
            busesCount = _allStatistics[0].GetPlayerDataAtIndex(i).BusesCount;
            expenses = 0;
            averageExpenses = 0;
            
            minValue = 1000f;
            maxValue = 0f;

            foreach (Statistics statistics in _allStatistics)
            {
                player = statistics.GetPlayerDataAtIndex(i);

                personalExpenses = player.PassengersArrangeCount * _priceOfPassengersArrange;
                personalExpenses += player.BusShuffleCount * _priceOfBusShuffle;

                expenses += personalExpenses;
                values.Add(expenses);

                minValue = Mathf.Min(minValue, personalExpenses);
                maxValue = Mathf.Max(maxValue, personalExpenses);
            }

            averageExpenses = expenses / _allStatistics.Count;
            _averageExpensesAtLevels.Add(averageExpenses);
            _averageExpensesAtLevelsForBus.Add(averageExpenses / busesCount);
            _minExpensesAtLevels.Add(minValue);
            _maxExpensesAtLevels.Add(maxValue);
            _minExpensesAtLevelsForBus.Add(minValue / busesCount);
            _maxExpensesAtLevelsForBus.Add(maxValue / busesCount);
        }
    }

    private void ExportStatistics()
    {
        List<string> report = new();
        string line;

        for (int i = 0; i < _levelsCount; i++)
        {
            line = $"Ср.потрачено: {_averageExpensesAtLevels[i]}," +
                    $"среднее на 1 автобус {Math.Round((decimal)_averageExpensesAtLevelsForBus[i], 2)} " +
                    $"(min: {_minExpensesAtLevels[i]}, max: {_maxExpensesAtLevels[i]})";

            report.Add(line);
        }

        report.Add("----------------------------------------------");

        float averageExpenses = GetAverageValue(_averageExpensesAtLevelsForBus);
        float minExpenses = GetAverageValue(_minExpensesAtLevelsForBus);
        float maxExpenses = GetAverageValue(_maxExpensesAtLevelsForBus);

        report.Add($"Средний коэф.: {averageExpenses} | мин.: {minExpenses} | макс.: {maxExpenses}");

        StatisticsSaver.ExportStatistics(report);
    }

    private float GetAverageValue(List<float> dataList)
    {
        float totalData = 0f;

        foreach (float data in dataList)
            totalData += data;

        float result = totalData / dataList.Count;

        return (float)Math.Round((decimal)result, 2);
    }
}
