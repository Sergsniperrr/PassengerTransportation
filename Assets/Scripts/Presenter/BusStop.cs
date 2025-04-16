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

    private readonly float _delayOfUpdateQueue = 0.01f;

    private BusStopNavigator _navigator;
    private ColorAnalyzer _colorAnalyzer;
    private bool[] _reservations;
    private Bus[] _stops;
    private float _updateCounter;

    public bool IsFreeStops => _reservations.Where(place => place == true).Count() > 0;

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
        if (_updateCounter < 0f)
        {
            _colorAnalyzer.TrySendPassengerToPlatform(_queue, _stops);
            _updateCounter = _delayOfUpdateQueue;
        }

        _updateCounter -= Time.deltaTime;
    }

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
    }
}
