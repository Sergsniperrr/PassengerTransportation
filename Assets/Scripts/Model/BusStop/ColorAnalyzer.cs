using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAnalyzer : MonoBehaviour
{
    private const int FailedIndex = -1;

    public void TrySendPassengerToPlatform(PassengerQueue queue, Bus[] stops)
    {
        int stopIndex = GetStopIndexWithBusOfDesiredColor(queue, stops);

        if (stopIndex == FailedIndex || stops[stopIndex] == null)
            return;

        if (stops[stopIndex].IsEmptySeat)
            queue.Dequeue().GetOnBus(stops[stopIndex]);
    }

    private int GetStopIndexWithBusOfDesiredColor(PassengerQueue queue, Bus[] stops)
    {
        for (int i = 0; i < stops.Length; i++)
        {
            if (stops[i] == null || queue.LastPassenger == null)
                continue;

            if (stops[i].Material == queue.LastPassenger.Material)
                return i;
        }

        return FailedIndex;
    }
}
