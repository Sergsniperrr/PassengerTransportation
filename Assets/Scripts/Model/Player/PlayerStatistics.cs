using System;
using UnityEngine;
using YG;

namespace Scripts.Model.Player
{
    [RequireComponent(typeof(PlayerStatisticsCollector))]
    public class PlayerStatistics : MonoBehaviour
    {
        private const int BusCountAtOneAd = 20;
        private const float PlayerSkillMultiplier = 0.95f;
        private const int MinLevelToStartCalculatingPlayerSkill = 10;

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

            BusesCount = busesCount;

            if (level < MinLevelToStartCalculatingPlayerSkill)
                return;

            TotalBusesCount += busesCount;
            _statisticsCollector.ResetValues();
        }

        public void FinishCollectData(int level)
        {
            if (level < MinLevelToStartCalculatingPlayerSkill)
                return;

            TotalAdsViewsCount += _statisticsCollector.AdsViewCount;
            CalculatePlayerSkill();
            MoneySpent = _statisticsCollector.MoneySpent;
        }

        private void CalculatePlayerSkill()
        {
            _adsViewRatio = (float)BusCountAtOneAd / TotalBusesCount * TotalAdsViewsCount;

            if (_adsViewRatio > 1f)
                IncreasePlayerSkill();

            if (_adsViewRatio < 1f)
                DecreasePlayerSkill();
        }

        private void IncreasePlayerSkill() =>
            PlayerSkill /= PlayerSkillMultiplier;

        private void DecreasePlayerSkill() =>
            PlayerSkill *= PlayerSkillMultiplier;
    }
}