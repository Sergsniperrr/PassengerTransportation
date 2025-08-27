using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CoinsView))]
[RequireComponent(typeof(PlayerStatistics))]
public class CoinsHandler : MonoBehaviour
{
    [field: SerializeField] public MoneyCounter Money { get; private set; }
    [field: SerializeField] public ScoreCounter Score { get; private set; }

    private readonly float _moneyRatio = 1f;

    private PlayerStatistics _statistics;
    private CoinsView _countersView;
    private int _level;

    public int TemporaryMoney { get; private set; }
    public int TemporaryScore { get; private set; }
    public int MoneyBuffer { get; private set; }

    private void Awake()
    {
        _countersView = GetComponent<CoinsView>();
        _statistics = GetComponent<PlayerStatistics>();
    }

    public void ShowCounters() =>
        _countersView.Show();

    public void HideCounters() =>
        _countersView.Hide();

    public void InitializeNewLevel(int level, int busCount)
    {
        if (busCount < 0)
            throw new ArgumentOutOfRangeException(nameof(busCount));

        if (level < 0)
            throw new ArgumentOutOfRangeException(nameof(level));


        //MoneyBuffer = Money.Coins;
        Money.SetValue(MoneyBuffer);
        _statistics.StartCollectData(busCount);
        _level = level;
        //_moneyBuffer = (int)Mathf.Round(busCount * _moneyRatio);
    }

    public void CompleteLevel()
    {
        _statistics.FinishCollectData();

        TemporaryMoney = CoinsCalculator.CalculateMoney(_level, _statistics.BusesCount, _statistics.PlayerSkill);
        TemporaryScore = CoinsCalculator.CalculateScore(_statistics.BusesCount,
                                                         _statistics.MoneySpent,
                                                         _statistics.PlayerSkill);
    }

    public void TransferCoins(int money, int score)
    {

    }

    private int CalculateMoney()
    {
        return 0;
    }

    private int CalculateScore()
    {
        return 0;
    }
}
