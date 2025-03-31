using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BusStopNavigator))]
public class BusStop : MonoBehaviour
{
    private BusStopNavigator _navigator;
    private bool[] _stops = new bool[7];
    private int _initialFreeStopsCount = 7;

    public bool IsFreeStops => _stops.Where(value => value == true).Count() > 0;

    private void Awake()
    {
        _navigator = GetComponent<BusStopNavigator>();

        for (int i = 0; i < _initialFreeStopsCount; i++)
            _stops[i] = true;
    }

    public Vector3 GetPointerCoordinate(int stopIndex) =>
        _navigator.GetPointerCoordinate(stopIndex);

    public Vector3 GetStopCoordinate(int stopIndex) =>
        _navigator.GetStopCoordinate(stopIndex);

    public int GetFreeStopIndex()
    {
        int failedIndex = -1;

        for (int i = 0; i < _stops.Length; i++)
        {
            if (_stops[i] == true)
            {
                _stops[i] = false;
                return i;
            }
        }

        return failedIndex;
    }

    public void ReleaseStop(int index)
    {
        if (index < 0 || index >= _stops.Length)
            throw new ArgumentOutOfRangeException(nameof(index));

        _stops[index] = true;
    }
}
