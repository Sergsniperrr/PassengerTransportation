using UnityEngine;

public struct BusStopSpot
{
    [field: SerializeField] public bool IsReserved { get; private set; }
    [field: SerializeField] public BusInBusStop Bus { get; private set; }

    public BusStopSpot (bool isFree, BusInBusStop bus)
    {
        IsReserved = isFree;
        Bus = bus;
    }
}
