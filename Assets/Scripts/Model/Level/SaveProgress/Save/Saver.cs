using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class Saver : MonoBehaviour
{
    [SerializeField] private Colors _colors;
    [SerializeField] private CoinsHandler _coins;
    [SerializeField] private ColorsHandler _colorsHandler;
    [SerializeField] private ElevatorsHandler _elevators;
    [SerializeField] private BusStop _busStop;
    [SerializeField] private Level _level;
    [SerializeField] private UndergroundBuses _undergroundBuses;
    [SerializeField] private PassengerQueue _queue;

    private Save _saveData;

    private Bus _bus;

    public void Save()
    {

        _saveData = InitialSave();
        //_saveData.SetVisualBuses(CreateVisibleBuses());
        _saveData.SetElevators(CreateElevators());
        _saveData.SetUndergroundBuses(CreateUndergroundBuses());
        _saveData.SetBusStopSpots(CreateBusStopSpots());

        //YG2.saves.SaveLocalProgress(_saveData);

        YG2.SaveProgress();
    }

    public void ResetLocalSave()
    {
        //Save save = YG2.saves.LocalProgress;

        //save.Reset();

        //YG2.saves.SaveLocalProgress(save);
    }

    private Save InitialSave()
    {
        List<int> colors = new();

        foreach (Passenger passenger in _queue.Passengers)
            colors.Add(_colors.GetIndexOfMaterial(passenger.Material));

        colors.AddRange(_colors.GetMaterialsIndexes(_colorsHandler.Colors));

        Debug.Log($"Money: {_coins.Money.Count}, Buffer: {_coins.MoneyBuffer}, colors count: {colors.Count}");

        return new Save(_coins.Money.Count, colors.ToArray(), CreateVisibleBuses());
    }

    private VisibleBus[] CreateVisibleBuses()
    {
        List<VisibleBus> buses = new();
        BusData busData;

        foreach (Bus bus in _level.Buses)
        {
            busData = new BusData(bus.SeatsCount, bus.transform.position, bus.transform.rotation);
            buses.Add(new VisibleBus(busData, _colors.GetIndexOfMaterial(bus.Material)));
        }

        return buses.ToArray();
    }

    private ElevatorData[] CreateElevators()
    {
        List<ElevatorData> elevators = new();
        ElevatorData elevatorData;

        foreach (Elevator elevator in _elevators.Elevators)
        {
            elevatorData = new ElevatorData(elevator.Name, elevator.transform.position, elevator.Counter.Count);
            elevators.Add(elevatorData);
        }

        return elevators.ToArray();
    }

    private UndergroundBus[] CreateUndergroundBuses()
    {
        List<UndergroundBus> buses = new();
        UndergroundBus undergroundBus;

        foreach (BusUnderground bus in _undergroundBuses.Buses)
        {
            undergroundBus = new UndergroundBus(bus.SeatsCount, _colors.GetIndexOfMaterial(bus.Material));
            buses.Add(undergroundBus);
        }

        return buses.ToArray();
    }

    private BusStopSpot[] CreateBusStopSpots()
    {
        int spotsCount = 7;
        BusStopSpot[] spots = new BusStopSpot[spotsCount];

        for (int i = 0; i < spotsCount; i++)
        {
            _bus = _busStop.GetBus(i);

            if (_busStop.GetReservation(i) || _bus == null)
            {
                spots[i] = GetDefaultBusStopSpot();
                continue;
            }

            spots[i] = BuildBusStopSpot(_bus);
        }

        return spots;
    }

    private BusStopSpot GetDefaultBusStopSpot()
    {
        int seatsCount = 4;
        BusData busData = new(seatsCount, Vector3.zero, Quaternion.identity);
        VisibleBus visibleBus = new(busData, 0);
        BusInBusStop busInBusStop = new(visibleBus, 0);

        return new BusStopSpot(true, busInBusStop);
    }

    private BusStopSpot BuildBusStopSpot(Bus bus)
    {
        if (bus == null)
            throw new ArgumentNullException(nameof(bus));

        BusData busData = new(bus.SeatsCount, bus.transform.position, bus.transform.rotation);
        VisibleBus visibleBus = new(busData, _colors.GetIndexOfMaterial(bus.Material));
        BusInBusStop busInBusStop = new(visibleBus, bus.SeatsCount - bus.FreeSeatsCount);

        return new BusStopSpot(false, busInBusStop);
    }
}
