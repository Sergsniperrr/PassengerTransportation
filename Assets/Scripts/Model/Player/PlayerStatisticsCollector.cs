using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatisticsCollector : MonoBehaviour
{
    [SerializeField] private PassengerColorShuffler _passengerShuffler;
    [SerializeField] private BusColorsShuffler _busShuffler;
    [SerializeField] private PassengerColorShuffler _viewingAdsInWarningWindow;
    [SerializeField] private Button _buttonViewAds;
    [SerializeField] private Prices _prices;

    public int MoneySpent { get; private set; }
    public int AdsViewCount { get; private set; }

    private void OnEnable()
    {
        _passengerShuffler.PassengersArranged += AddPassengerArrangeCost;
        _busShuffler.BusesShuffled += AddBusShuffleCost;
        _viewingAdsInWarningWindow.PassengersArranged += HandleViewAdsInWarningWindow;
        _buttonViewAds.onClick.AddListener(AddOneViewAd);
    }

    private void OnDisable()
    {
        _passengerShuffler.PassengersArranged -= AddPassengerArrangeCost;
        _busShuffler.BusesShuffled -= AddBusShuffleCost;
        _viewingAdsInWarningWindow.PassengersArranged -= HandleViewAdsInWarningWindow;
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
