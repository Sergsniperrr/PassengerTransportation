using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BusPointsCalculator))]
[RequireComponent(typeof(ColorAnalyzer))]
public class BusStop : MonoBehaviour, IBusReceiver
{
    private const int FailedIndex = -1;

    [SerializeField] private int _stopsCount = 7;
    [SerializeField] private int _initialFreeStopsCount = 7;
    [SerializeField] private PassengerQueue _queue;

    private readonly float _delayOfUpdateQueue = 0.06f;
    private readonly WaitForSeconds _waitForCheckGameOver = new(1f);

    private ColorAnalyzer _colorAnalyzer;
    private WaitForSeconds _delayBeforeLeave = new(0.3f);
    private bool[] _reservations;
    private Bus[] _stops;
    private Bus _outGoingBus;
    private Queue<Bus> _outGoingBuses = new();
    private float _updateCounter;
    private bool _canLeave = true;
    private bool _isFreePlace = true;

    public event Action<Bus> BusReceived;
    public event Action PassengerLeft;
    public event Action AllPlacesOccupied;

    public int StopsCount => _stopsCount;

    private void Awake()
    {
        if (_initialFreeStopsCount > _stopsCount)
            throw new IndexOutOfRangeException(nameof(_initialFreeStopsCount));

        _colorAnalyzer = GetComponent<ColorAnalyzer>();

        _reservations = new bool[_stopsCount];
        _stops = new Bus[_stopsCount];

        for (int i = 0; i < _initialFreeStopsCount; i++)
            _reservations[i] = true;
    }

    private void Update()
    {
        if (_updateCounter > 0)
        {
            _updateCounter -= Time.deltaTime;
        }
        else
        {
            HandlePassengersBoarding();
            HandleBusesMoveOut();

            _updateCounter = _delayOfUpdateQueue;
        }
    }

    public Bus GetBusOnStopByIndex(int index) =>
        _stops[index];

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

    public void TakeBus(Bus bus, int platformIndex)
    {
        if (platformIndex < 0 || platformIndex >= _stops.Length)
            throw new ArgumentOutOfRangeException(nameof(platformIndex));

        _stops[platformIndex] = bus;
        BusReceived?.Invoke(bus);

        bus.FillingCompleted += AddToLeavingBusesQueue;
    }

    public void HandlePassengersBoarding()
    {
        if (_queue.LastPassenger != null && _queue.LastPassenger.IsFinishedMovement)
        {
            int stopIndex = _colorAnalyzer.TrySendPassengerToPlatform(_queue.LastPassenger.Material, _stops);

            if (stopIndex != FailedIndex)
            {
                bool isEmptySeat = _queue.LastPassenger.TryGetOnBus(_stops[stopIndex]);

                if (isEmptySeat)
                {
                    _queue.LastPassenger.GotOnBus += GetOnBus;
                    _queue.RemoveLastPassenger();
                }
            }
            else
            {
                HandleGameOver();
            }
        }
    }

    public void ResetFreePlaces() =>
        _isFreePlace = true;

    private void GetOnBus(Passenger passenger)
    {
        passenger.GotOnBus -= GetOnBus;

        passenger.Bus.TakePassenger(passenger);
        PassengerLeft?.Invoke();
    }

    private void HandleBusesMoveOut()
    {
        if (_canLeave && _outGoingBuses.Count > 0)
        {
            _outGoingBus = _outGoingBuses.Dequeue();
            _outGoingBus.MoveOutFromBusStop();
            _canLeave = false;
            StartCoroutine(AllowLeave());
        }
    }

    private void AddToLeavingBusesQueue(Bus bus)
    {
        _outGoingBuses.Enqueue(bus);

        bus.FillingCompleted -= AddToLeavingBusesQueue;
    }

    private IEnumerator AllowLeave()
    {
        yield return _delayBeforeLeave;

        _canLeave = true;
    }

    private void HandleGameOver()
    {
        if (_isFreePlace == false)
            return;

        if (_stops.Contains(null))
            return;

        _isFreePlace = false;

        StartCoroutine(HandleGameOverAfterDelay());
    }

    private IEnumerator HandleGameOverAfterDelay()
    {
        yield return _waitForCheckGameOver;

        if (_stops.Contains(null) == false &&
            _colorAnalyzer.CheckDesiredColor(_queue.LastPassenger.Material, _stops) == false)
        {
            AllPlacesOccupied?.Invoke();
        }
        else
        {
            ResetFreePlaces();
        }
    }
}
