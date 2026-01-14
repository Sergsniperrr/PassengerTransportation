using Scripts.Model.Coins;
using UnityEngine;
using YG;

namespace Scripts.Model.Player
{
    public class PlayerSaver : MonoBehaviour
    {
        [SerializeField] private CoinsHandler _coinsHandler;
        [SerializeField] private PlayerStatistics _playerStatistics;

        public void Save()
        {
            YG2.saves.WriteLevel(_coinsHandler.Level + 1);
            YG2.saves.WriteMoney(_coinsHandler.Money.Count);
            YG2.saves.WriteScore(_coinsHandler.Score.Count);
            YG2.saves.WriteTotalBusesCount(_playerStatistics.TotalBusesCount);
            YG2.saves.WriteTotalAdsViewsCount(_playerStatistics.TotalAdsViewsCount);
            YG2.saves.WritePlayerSkill(_playerStatistics.PlayerSkill);
        }
    }
}