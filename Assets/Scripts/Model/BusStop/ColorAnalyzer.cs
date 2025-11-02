using System.Linq;
using UnityEngine;

public class ColorAnalyzer : MonoBehaviour
{
    private const int FailedIndex = -1;

    public int TrySendPassengerToPlatform(Material passengerColor, Spot[] spots)
    {
        int stopIndex = GetStopIndexWithBusOfDesiredColor(passengerColor, spots);

        if (stopIndex == FailedIndex || spots[stopIndex].BusAtBusStop == null)
            return FailedIndex;

        if (spots[stopIndex].BusAtBusStop.IsEmptySeat)
            return stopIndex;

        return FailedIndex;
    }

    public bool CheckDesiredColor(Material passengerColor, Spot[] spots) =>
        spots.Any(spot => spot.BusAtBusStop.Material == passengerColor);

    private int GetStopIndexWithBusOfDesiredColor(Material passengerColor, Spot[] spots)
    {
        for (int i = 0; i < spots.Length; i++)
        {
            if (spots[i].BusAtBusStop == null || passengerColor == null)
                continue;

            if (spots[i].BusAtBusStop.Material == passengerColor)
                return i;
        }

        return FailedIndex;
    }
}
