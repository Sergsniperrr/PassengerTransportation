using System;
using System.Collections.Generic;
using UnityEngine;

public class PassengerColorArranger : MonoBehaviour
{
    [SerializeField] private PassengerQueue _queue;
    [SerializeField] private BusStop _busStop;
    [SerializeField] private ColorsHandler _colorHandler;
    [SerializeField] private ColorAnalyzer _colors;

    public event Action PassengersArranged;

    public void ArrangeColors()
    {
        Queue<Material> freePlacesColors = _colors.GetAllFreeColors();
        List<Material> colors = _colorHandler.Colors;
        Passenger[] visibleQueue = _queue.Passengers;
        Material materialBuffer;

        foreach (Passenger passenger in visibleQueue)
        {
            if (passenger == null)
                continue;

            colors.Add(passenger.Material);
        }


        foreach (Passenger passenger in visibleQueue)
        {
            if (freePlacesColors.Count > 0)
            {
                materialBuffer = freePlacesColors.Dequeue();
                colors.Remove(materialBuffer);
                passenger.SetColor(materialBuffer);

            }
            else
            {
                passenger.SetColor(colors[0]);
                colors.RemoveAt(0);
            }
        }

        _colorHandler.SetColorsQueue(colors);
        _busStop.ResetFreePlaces();
        _busStop.HandlePassengersBoarding();

        PassengersArranged?.Invoke();
    }
}
