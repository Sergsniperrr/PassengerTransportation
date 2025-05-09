using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BusStopNavigator))]
[RequireComponent(typeof(ColorAnalyzer))]
public class BusStop : MonoBehaviour
{
    private const int FailedIndex = -1;

    [SerializeField] private int _stopsCount = 7;
    [SerializeField] private int _initialFreeStopsCount = 7;
    [SerializeField] private PassengerQueue _queue;

    private readonly float _delayOfUpdateQueue = 0.05f;

    private BusStopNavigator _navigator;
    private ColorAnalyzer _colorAnalyzer;
    private bool[] _reservations;
    private Bus[] _stops;
    private Bus _outGoingBus;
    private Queue<Bus> _outGoingBuses = new();
    private float _updateCounter;
    private int _stopIndexBuffer;
    private bool _canLeave = true;

    public bool IsFreeStops => _reservations.Where(place => place == true).Count() > 0;
    public int StopsCount => _stopsCount;

    private void Awake()
    {
        if (_initialFreeStopsCount > _stopsCount)
            throw new IndexOutOfRangeException(nameof(_initialFreeStopsCount));

        _navigator = GetComponent<BusStopNavigator>();
        _colorAnalyzer = GetComponent<ColorAnalyzer>();

        _reservations = new bool[_stopsCount];
        _stops = new Bus[_stopsCount];

        for (int i = 0; i < _initialFreeStopsCount; i++)
            _reservations[i] = true;
    }

    private void Update()
    {
        HandlePassengersBoarding();
        HandleBusesMoveOut();
    }

    public Bus GetBusOnStopByIndex(int index) => 
        _stops[index];

    public Vector3 GetPointerCoordinate(int stopIndex) =>
        _navigator.GetPointerCoordinate(stopIndex);

    public Vector3 GetStopCoordinate(int stopIndex) =>
        _navigator.GetStopCoordinate(stopIndex);

    public int GetFreeStopIndex()
    {
        for (int i = 0; i < _stopsCount; i++)
        {
            if (_reservations[i])
            {
                _reservations[i] = false;
                return i;
            }
        }

        return FailedIndex;
    }

    public void ReleaseStop(int index)
    {
        if (index < 0 || index >= _stops.Length)
            throw new ArgumentOutOfRangeException(nameof(index));

        _reservations[index] = true;
        _stops[index] = null;
    }

    public void TakeBus(Bus bus, int placeIndex)
    {
        if (placeIndex < 0 || placeIndex >= _stops.Length)
            throw new ArgumentOutOfRangeException(nameof(placeIndex));

        _stops[placeIndex] = bus;

        bus.LoadCompleted += AddToLeavingBusesQueue;
    }

    private void HandlePassengersBoarding()
    {
        if (_updateCounter < 0f && _queue.LastPassenger != null && _queue.LastPassenger.IsFinishedMovement)
        {
            _stopIndexBuffer = _colorAnalyzer.TrySendPassengerToPlatform(_queue.LastPassenger.Material, _stops);

            if (_stopIndexBuffer != FailedIndex)
            {
                _queue.LastPassenger.GetOnBus(_stops[_stopIndexBuffer]);
                _queue.Dequeue();
            }

            _updateCounter = _delayOfUpdateQueue;
        }

        _updateCounter -= Time.deltaTime;
    }

    private void HandleBusesMoveOut()
    {
        if (_canLeave && _outGoingBuses.Count > 0)
        {
            _outGoingBus = _outGoingBuses.Dequeue();
            _outGoingBus.MoveOutFromBusStop();
            _canLeave = false;

            _outGoingBus.StopReleased += AllowExit;
        }
    }

    private void AllowExit(Bus bus, int _)
    {
        _canLeave = true;

        _outGoingBus.StopReleased += AllowExit;
    }

    private void AddToLeavingBusesQueue(Bus bus)
    {
        _outGoingBuses.Enqueue(bus);

        bus.LoadCompleted -= AddToLeavingBusesQueue;
    }
}
