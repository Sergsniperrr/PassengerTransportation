using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Colors))]
public class BusSpawner : MonoBehaviour
{
    [SerializeField] private Bus _smallBusPrefab;
    [SerializeField] private Bus _middleBusPrefab;
    [SerializeField] private Bus _bigBusPrefab;
    [SerializeField] private ElevatorsHandler _elevators;
    [SerializeField] private UndergroundBuses _undergroundBuses;
    [SerializeField] private BusStop _busStop;
    [SerializeField] private Effects _effects;
    [SerializeField] private BusPointsCalculator _busNavigator;
    [SerializeField] private int _smallBusSeatsCount = 4;
    [SerializeField] private int _middleBusSeatsCount = 6;
    [SerializeField] private int _bigBusSeatsCount = 10;
    [SerializeField] private BusColorsShuffler _busColorShuffler;

    private Dictionary<int, Bus> _prefabs = new();
    private LevelsDataContainer _levels;
    private Colors _colors;

    public event Action<Bus> BusUndergroundSpawned;
    public event Action<Bus> BusLeftParkingLot;

    public UndergroundBuses UndergroundBuses => _undergroundBuses;

    private void Awake()
    {
        _prefabs.Add(_smallBusSeatsCount, _smallBusPrefab);
        _prefabs.Add(_middleBusSeatsCount, _middleBusPrefab);
        _prefabs.Add(_bigBusSeatsCount, _bigBusPrefab);

        _colors = GetComponent<Colors>();
    }

    private void OnEnable()
    {
        _elevators.ElevatorReleased += SpawnFromUnderground;
    }

    private void OnDisable()
    {
        _elevators.ElevatorReleased -= SpawnFromUnderground;
    }

    public List<Bus> SpawnLevel(BusData[] levelData)
    {
        List<Bus> buses = new();
        Bus bus;

        foreach (BusData busData in levelData)
        {
            bus = Spawn(busData);
            buses.Add(bus);

            bus.LeftParkingLot += SendBusForRemove;
        }

        return buses;
    }

    public void InitializeUndergroundBuses(LevelBusCalculator calculator, BusData[] levelData)
    {
        BusData[] bigBuses = levelData.Where(bus => bus.SeatsCount == _bigBusSeatsCount).ToArray();

        _elevators.InitializeData(bigBuses, calculator.GetElevatorsCount(), calculator.UndergroundBusesCount);
        _undergroundBuses.Generate(calculator.UndergroundBusesCount);
        _busColorShuffler.InitializeUndergroundBuses(_undergroundBuses);
    }

    public void SpawnFromUnderground(Elevator elevator)
    {
        BusUnderground busData = _undergroundBuses.Dequeue();
        Bus bus = Spawn(elevator.ReleaseBus(busData));

        BusUndergroundSpawned?.Invoke(bus);

        bus.gameObject.SetActive(false);
        bus.SetColor(busData.Material);
        bus.Disable();
        elevator.LiftBus(bus);

        elevator.BusLifted += ActivateBus;
    }

    public void SpawnCreatedLevel(BusData[] levelData) // Only for making levels!!!
    {
        Bus[] buses = GetBuses();

        if (buses.Length > 0)
        {
            foreach (Bus bus in buses)
            {
                Debug.Log($"{bus.gameObject.name} has been destroyed!");
                Destroy(bus.gameObject);
            }

            Debug.Log($"{buses.Length} buses were destroyed.");
            Debug.Log("---------------------------------------\n");
        }

        foreach (BusData busData in levelData)
        {
            //Spawn(busData);
            Debug.Log($"{Spawn(busData).name} has been spawned.");
        }

        Debug.Log($"{levelData.Length} buses were spawned.");
    }

    public Bus[] GetBuses() =>
        GetComponentsInChildren<Bus>();

    public void StartElevators() =>
        _elevators.DetectInitialBuses();

    private Bus Spawn(BusData busData)
    {
        Bus bus = Instantiate(_prefabs[busData.SeatsCount], busData.Position, busData.Rotation);
        bus.transform.SetParent(transform);
        bus.InitializeData(_busStop, _busNavigator, _effects);
        bus.SetColor(_colors.GetRandomColor());

        return bus;
    }

    private void ActivateBus(Bus bus, Elevator elevator)
    {
        elevator.BusLifted -= ActivateBus;

        bus.Activate();
        bus.LeftParkingLot += SendBusForRemove;

        //BusUndergroundSpawned?.Invoke(bus);
    }

    private void SendBusForRemove(Bus bus)
    {
        bus.LeftParkingLot -= SendBusForRemove;

        BusLeftParkingLot?.Invoke(bus);
    }
}
