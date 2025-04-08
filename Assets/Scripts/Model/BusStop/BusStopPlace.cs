using System;

public class BusStopPlace
{
    public Bus Bus;
    public bool IsFree = true;

    public void Reserve() =>
        IsFree = false;

    public void AddBus(Bus bus)
    {
        if (Bus != null)
            throw new Exception("Place at the bus stop is already occupied!");

        Bus = bus != null ? bus : throw new ArgumentNullException(nameof(bus));
    }

    public void Free()
    {
        IsFree = true;
        Bus = null;
    }
}
