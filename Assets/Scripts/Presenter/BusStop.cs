using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BusStopNavigator))]
public class BusStop : MonoBehaviour
{
    [SerializeField] private int _stopsCount = 7;
    [SerializeField] private int _initialFreeStopsCount = 7;

    private BusStopNavigator _navigator;
    private BusStopPlace[] _stops;
    public bool IsFreeStops => _stops.Where(place => place.IsFree == true).Count() > 0;


    private void Awake()
    {
        if (_initialFreeStopsCount > _stopsCount)
            throw new IndexOutOfRangeException(nameof(_initialFreeStopsCount));

        _navigator = GetComponent<BusStopNavigator>();

        _stops = new BusStopPlace[_stopsCount];

        for (int i = 0; i < _initialFreeStopsCount; i++)
            _stops[i] = new BusStopPlace();
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
            if (_stops[i].IsFree == true)
            {
                _stops[i].Reserve();
                return i;
            }
        }

        return failedIndex;
    }

    public void ReleaseStop(int index)
    {
        if (index < 0 || index >= _stops.Length)
            throw new ArgumentOutOfRangeException(nameof(index));

        _stops[index].Free();
    }

    public void TakeBus(Bus bus, int placeIndex)
    {
        if (placeIndex < 0 || placeIndex >= _stops.Length)
            throw new ArgumentOutOfRangeException(nameof(placeIndex));

        _stops[placeIndex].AddBus(bus);
    }
}
