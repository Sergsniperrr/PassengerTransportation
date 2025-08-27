using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatisticsCollector : MonoBehaviour
{
    [SerializeField] private PassengerColorShuffler _passengerShuffler;
    [SerializeField] private BusColorsShuffler _busShuffler;
    [SerializeField] private PassengerColorShuffler _viewingAds;
    [SerializeField] private Prices _prices;

    public int MoneySpent { get; private set; }
    public int AdsViewCount { get; private set; }

    private void OnEnable()
    {
        _passengerShuffler.PassengersArranged += AddPassengerShuffleCost;
        _viewingAds.PassengersArranged += AddPassengerShuffleCost;
        _busShuffler.BusesShuffled += AddBusShuffleCost;
    }

    private void OnDisable()
    {
        _passengerShuffler.PassengersArranged -= AddPassengerShuffleCost;
        _viewingAds.PassengersArranged -= AddPassengerShuffleCost;
        _busShuffler.BusesShuffled -= AddBusShuffleCost;
    }

    public void ResetValues()
    {
        MoneySpent = 0;
        AdsViewCount = 0;
    }

    private void AddPassengerShuffleCost() =>
        MoneySpent += _prices.ArrangingPassengers;

    private void AddBusShuffleCost() =>
        MoneySpent += _prices.ShufflingBuses;
}
