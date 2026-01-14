using System;
using UnityEngine;

namespace YG
{
    public partial class SavesYG
    {
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public int Money { get; private set; }
        [field: SerializeField] public int Score { get; private set; }
        [field: SerializeField] public int TotalBusesCount { get; private set; }
        [field: SerializeField] public int TotalAdsViewsCount { get; private set; }
        [field: SerializeField] public float PlayerSkill { get; private set; }

        public void WriteLevel(int level) =>
            Level = level >= 0 ? level : throw new ArgumentOutOfRangeException(nameof(level));

        public void WriteMoney(int value) =>
            Money = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));

        public void WriteScore(int value) =>
            Score = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));

        public void WriteTotalBusesCount(int count) =>
            TotalBusesCount = count >= 0 ? count : throw new ArgumentOutOfRangeException(nameof(count));

        public void WriteTotalAdsViewsCount(int count) =>
            TotalAdsViewsCount = count >= 0 ? count : throw new ArgumentOutOfRangeException(nameof(count));

        public void WritePlayerSkill(float playerSkill) =>
            PlayerSkill = playerSkill >= 0f ? playerSkill : throw new ArgumentOutOfRangeException(nameof(playerSkill));
    }
}