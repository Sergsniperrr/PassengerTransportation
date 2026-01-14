using Scripts.View.Buses;
using Scripts.View.Color;
using Scripts.View.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Model.Player
{
    public class PlayerStatisticsCollector : MonoBehaviour
    {
        [SerializeField] private PassengerColorArranger _passengerShuffler;
        [SerializeField] private BusColorsShuffler _busShuffler;
        [SerializeField] private WarningWindow _warningWindow;
        [SerializeField] private Button _buttonViewAds;
        [SerializeField] private Prices _prices;

        public int MoneySpent { get; private set; }
        public int AdsViewCount { get; private set; }

        private void OnEnable()
        {
            _passengerShuffler.PassengersArranged += AddPassengerArrangeCost;
            _busShuffler.BusesShuffled += AddBusShuffleCost;
            _warningWindow.AdViewed += HandleViewAdsInWarningWindow;
            _buttonViewAds.onClick.AddListener(AddOneViewAd);
        }

        private void OnDisable()
        {
            _passengerShuffler.PassengersArranged -= AddPassengerArrangeCost;
            _busShuffler.BusesShuffled -= AddBusShuffleCost;
            _warningWindow.AdViewed -= HandleViewAdsInWarningWindow;
            _buttonViewAds.onClick.RemoveListener(AddOneViewAd);
        }

        public void ResetValues()
        {
            MoneySpent = 0;
            AdsViewCount = 0;
        }

        private void AddPassengerArrangeCost() =>
            MoneySpent += _prices.ArrangingPassengers;

        private void AddBusShuffleCost() =>
            MoneySpent += _prices.ShufflingBuses;

        private void HandleViewAdsInWarningWindow()
        {
            AddPassengerArrangeCost();
            AddOneViewAd();
        }

        private void AddOneViewAd() =>
            AdsViewCount++;
    }
}