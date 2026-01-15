using Scripts.Model.Money;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Scripts.Model.Other
{
    public class ViewAds : MonoBehaviour
    {
        private const float TransferDuration = 1.5f;
        private const string RewardId = "Money";

        [SerializeField] private Button _button;
        [SerializeField] private Prices _prices;
        [SerializeField] private MoneyCounter _money;

        private void OnEnable()
        {
            _button.onClick.AddListener(ViewAd);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ViewAd);
        }

        private void ViewAd()
        {
            YG2.RewardedAdvShow(RewardId, TakeReward);
        }

        private void TakeReward()
        {
            _money.Add(_prices.ViewingAd, TransferDuration);
        }
    }
}