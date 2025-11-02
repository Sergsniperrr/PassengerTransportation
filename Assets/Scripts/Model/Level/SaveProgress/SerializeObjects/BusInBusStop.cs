using System;
using UnityEngine;

[Serializable]
public struct BusInBusStop
{
    [field: SerializeField] public VisibleBus VisibleBus { get; }
    [field: SerializeField] public int PassengersCount { get; }

    public BusInBusStop (VisibleBus bus, int passengersCount)
    {
        VisibleBus = bus;
        PassengersCount = passengersCount;
    }
}
