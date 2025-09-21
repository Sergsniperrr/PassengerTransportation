using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorsHandler : MonoBehaviour
{
    [SerializeField] private ElevatorSpawner _spawner;
    [SerializeField] private Bus[] _buses;

    private List<Elevator> _elevators = new();

    public event Action<Elevator> ElevatorReleased;

    private void OnDisable()
    {
        foreach (Elevator elevator in _elevators)
            elevator.Released -= ReleaseElevator;
    }

    public void InitializeData(BusData[] bigBuses, int elevatorsCount, int busesCount)
    {
        if (_elevators.Count > 0)
        {
            foreach (Elevator elevator in _elevators)
                Destroy(elevator.gameObject);

            _elevators.Clear();
        }

        elevatorsCount = Mathf.Min(elevatorsCount, bigBuses.Length);
        int[] elevatorsCounters = CalculateBusesInElevators(busesCount, elevatorsCount);

        if (elevatorsCounters == null)
            return;

        var busesAtElevators = bigBuses
            .OrderBy(Bus => UnityEngine.Random.value)
            .Take(elevatorsCount)
            .ToArray();

        for (int i = 0; i < elevatorsCount; i++)
            SpawnElevator(busesAtElevators[i], elevatorsCounters[i]);
    }

    public void DetectInitialBuses()
    {
        foreach (Elevator elevator in _elevators)
            elevator.DetectFirstBus();
    }

    private int[] CalculateBusesInElevators(int busesCount, int elevatorsCount)
    {
        if (elevatorsCount == 0)
            return null;

        int remnant = busesCount % elevatorsCount;
        int baseCountInElevator = busesCount / elevatorsCount;
        int[] result = new int[elevatorsCount];

        for (int i = 0; i < elevatorsCount; i++)
            result[i] = baseCountInElevator;

        for (int i = 0; i < remnant; i++)
            result[i] += 1;

        return result;
    }

    private void SpawnElevator(BusData initialBus, int busCount)
    {
        Elevator elevator = _spawner.Spawn(initialBus);
        elevator.SetBusesCounter(busCount);
        _elevators.Add(elevator);

        elevator.Released += ReleaseElevator;
    }

    private void ReleaseElevator(Elevator elevator)
    {
        ElevatorReleased?.Invoke(elevator);
    }
}
