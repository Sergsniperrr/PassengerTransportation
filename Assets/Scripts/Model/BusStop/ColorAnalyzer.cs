using UnityEngine;

public class ColorAnalyzer : MonoBehaviour
{
    private const int FailedIndex = -1;

    public int TrySendPassengerToPlatform(Material passengerColor, Bus[] stops)
    {
        int stopIndex = GetStopIndexWithBusOfDesiredColor(passengerColor, stops);

        if (stopIndex == FailedIndex || stops[stopIndex] == null)
            return FailedIndex;

        if (stops[stopIndex].IsEmptySeat)
            return stopIndex;

        return FailedIndex;
    }

    private int GetStopIndexWithBusOfDesiredColor(Material passengerColor, Bus[] stops)
    {
        for (int i = 0; i < stops.Length; i++)
        {
            if (stops[i] == null || passengerColor == null)
                continue;

            if (stops[i].Material == passengerColor)
                return i;
        }

        return FailedIndex;
    }
}
