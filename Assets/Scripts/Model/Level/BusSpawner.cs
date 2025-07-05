using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BusSpawner : MonoBehaviour
{
    [SerializeField] private Bus _smallBusPrefab;
    [SerializeField] private Bus _middleBusPrefab;
    [SerializeField] private Bus _bigBusPrefab;
    [SerializeField] private int _smallBusSeatsCount = 4;
    [SerializeField] private int _middleBusSeatsCount = 6;
    [SerializeField] private int _bigBusSeatsCount = 10;
    //[SerializeField] private LevelDataLoader _loader;

    private Dictionary<int, Bus> _prefabs = new();
    private LevelsDataContainer _levels;

    private void Awake()
    {
        _prefabs.Add(_smallBusSeatsCount, _smallBusPrefab);
        _prefabs.Add(_middleBusSeatsCount, _middleBusPrefab);
        _prefabs.Add(_bigBusSeatsCount, _bigBusPrefab);

        //_levels = LevelDataLoader.LoadData();
    }

    public List<Bus> SpawnLevel(BusData[] levelData)
    {
        //if (levelNumber < 0 && levelNumber >= _levels.Count)
        //    throw new ArgumentOutOfRangeException(nameof(levelNumber));


        //BusData[] levelData = _levels.GetLevel(levelNumber).Buses;
        List<Bus> buses = new();

        foreach (BusData busData in levelData)
            buses.Add(Spawn(busData));

        return buses;
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

    private Bus Spawn(BusData busData)
    {
        Bus bus = Instantiate(_prefabs[busData.SeatsCount], busData.Position, busData.Rotation);

        bus.transform.SetParent(transform);

        return bus;
    }
}
