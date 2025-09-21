using System;
using UnityEngine;
using YG;

[RequireComponent(typeof(PlayerStatisticsCollector))]
public class PlayerStatistics : MonoBehaviour
{
    private readonly int _busCountAtOneAd = 20;
    private readonly float _playerSkillMultiplier = 0.95f;
    private readonly int _minLevelToStartCalculatingPlayerSkill = 10;

    private PlayerStatisticsCollector _statisticsCollector;
    private float _adsViewRatio;

    public int TotalAdsViewsCount { get; private set; }
    public int TotalBusesCount { get; private set; }
    public float PlayerSkill { get; private set; } = 1f;
    public int MoneySpent { get; private set; }
    public int BusesCount { get; private set; }

    private void Awake()
    {
        _statisticsCollector = GetComponent<PlayerStatisticsCollector>();
    }

    private void Start()
    {
        TotalBusesCount = YG2.saves.TotalBusesCount;
        TotalAdsViewsCount = YG2.saves.TotalAdsViewsCount;
        PlayerSkill = YG2.saves.PlayerSkill;
    }

    public void StartCollectData(int busesCount, int level)
    {
        if (busesCount < 0)
            throw new ArgumentOutOfRangeException(nameof(busesCount));

        if (level < _minLevelToStartCalculatingPlayerSkill)
            return;

        TotalBusesCount += busesCount;
        _statisticsCollector.ResetValues();
        BusesCount = busesCount;
    }

    public void FinishCollectData(int level)
    {
        if (level < _minLevelToStartCalculatingPlayerSkill)
            return;

        TotalAdsViewsCount += _statisticsCollector.AdsViewCount;
        CalculatePlayerSkill();
        MoneySpent = _statisticsCollector.MoneySpent;
    }

    private void CalculatePlayerSkill()
    {
        _adsViewRatio = (float)_busCountAtOneAd / TotalBusesCount * TotalAdsViewsCount;

        if (_adsViewRatio > 1f)
            IncreasePlayerSkill();

        if (_adsViewRatio < 1f)
            DecreasePlayerSkill();
    }

    private void IncreasePlayerSkill() =>
        PlayerSkill /= _playerSkillMultiplier;

    private void DecreasePlayerSkill() =>
        PlayerSkill *= _playerSkillMultiplier;
}
