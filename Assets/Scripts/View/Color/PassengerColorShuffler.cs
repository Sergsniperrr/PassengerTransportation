using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassengerColorShuffler : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private PassengerQueue _queue;
    [SerializeField] private BusStop _busStop;
    [SerializeField] private ColorsHandler _colorHandler;

    private void OnEnable()
    {
        _button.onClick.AddListener(ShuffleColors);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ShuffleColors);
    }

    private void ShuffleColors()
    {
        Queue<Material> freePlacesColors = GetFreePlacesColorsOfBuses();
        List<Material> removableColors = new();
        List<Material> returnableColors = new();
        Passenger[] visibleQueue = _queue.Passengers;
        Material materialBuffer;

        foreach (Passenger passenger in visibleQueue)
        {
            if (passenger == null)
                continue;

            //returnableColors.Add(passenger.Material);
            _colorHandler.EnqueuePassengerColor(passenger.Material);

            if (freePlacesColors.Count > 0)
            {
                Debug.Log($"freePlacesColors = {freePlacesColors.Count}");

                materialBuffer = freePlacesColors.Dequeue();
                removableColors.Add(materialBuffer);
                passenger.SetColor(materialBuffer);

            }
            else
            {
                passenger.SetColor(_colorHandler.DequeuePassengerColor());
            }
        }

        _colorHandler.RemoveColors(removableColors.ToArray());

        //_colorHandler.ReplaceColors(returnableColors, removableColors);
    }

    //private void ShuffleColors()
    //{
    //    Queue<Material> freePlacesColors = GetFreePlacesColorsOfBuses();
    //    List<Material> removableColors = new();
    //    List<Material> returnableColors = new();
    //    Passenger[] visibleQueue = _queue.Passengers;
    //    Material materialBuffer;
        
    //    foreach (Passenger passenger in visibleQueue)
    //    {
    //        if (passenger == null)
    //            continue;

    //        returnableColors.Add(passenger.Material);

    //        if (freePlacesColors.Count > 0)
    //        {
    //            materialBuffer = freePlacesColors.Dequeue();
    //            removableColors.Add(materialBuffer);
    //        }
    //        else
    //        {
    //            materialBuffer = _colorHandler.DequeuePassengerColor();
    //        }

    //        passenger.SetColor(materialBuffer);
    //    }

    //    _colorHandler.ReplaceColors(returnableColors, removableColors);
    //}

    private Queue<Material> GetFreePlacesColorsOfBuses()
    {
        Queue<Material> colors = new();
        int stopsCount = _busStop.StopsCount;
        Bus bus;
        int freeSeatsCount;

        for (int i = 0; i < stopsCount; i++)
        {
            bus = _busStop.GetBusOnStopByIndex(i);

            if (bus == null || bus.FreeSeatsCount == 0)
                continue;

            freeSeatsCount = bus.FreeSeatsCount;

            for (int counter = 0; counter < freeSeatsCount; counter++)
                colors.Enqueue(bus.Material);
        }

        return colors;
    }
}
