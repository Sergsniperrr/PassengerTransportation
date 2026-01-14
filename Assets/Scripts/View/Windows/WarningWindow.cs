using System;
using Scripts.View.Color;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Scripts.View.Windows
{
    public class WarningWindow : SimpleWindow
    {
        private const string RewardId = "PassengerArrange";

        [SerializeField] private Button _passengersArrangingButton;
        [SerializeField] private PassengerColorArranger _colorArranger;

        public event Action AdViewed;

        public bool IsGameContinues { get; private set; }

        public override void Open(float delay = 0)
        {
            base.Open(delay);

            IsGameContinues = false;
            _passengersArrangingButton.onClick.AddListener(ViewAd);
        }

        private void ViewAd()
        {
            YG2.RewardedAdvShow(RewardId, TakeReward);
        }

        private void TakeReward()
        {
            _passengersArrangingButton.onClick.RemoveListener(ViewAd);

            _colorArranger.ArrangeColors();
            IsGameContinues = true;

            AdViewed?.Invoke();

            Close();
        }
    }
}