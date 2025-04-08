using UnityEngine;

[RequireComponent(typeof(MouseInputHandler))]
public class TransportationHandler : MonoBehaviour
{
    [SerializeField] private BusStop _busStop;
    [SerializeField] private PassengerQueue _queue;
    [SerializeField] private ColorsHandler _colorsHandler;
    [SerializeField] private Level _level;

    private MouseInputHandler _input;

    private void Awake()
    {
        _input = GetComponent<MouseInputHandler>();
    }

    private void OnEnable()
    {
        _input.BusSelected += RunBus;
    }

    private void OnDisable()
    {
        _input.BusSelected -= RunBus;
    }

    private void Start()
    {
        _level.SetBusesRandomColor();
        _colorsHandler.InitializeColors();
        _queue.InitializeColorsSpawner(_colorsHandler);
        _queue.Spawn();
    }

    private void RunBus(Bus bus)
    {
        if (_busStop.IsFreeStops)
        {
            bus.StopReleased += ReleaseStop;

            bus.SetStopIndex(_busStop.GetFreeStopIndex());
            AssignRouteForBus(bus);

            bus.Run();
        }
    }

    private void ReleaseStop(Bus bus, int index)
    {
        bus.StopReleased -= ReleaseStop;

        _busStop.ReleaseStop(index);
    }

    private void AssignRouteForBus(Bus bus)
    {
        Vector3 pointerCoordinate = _busStop.GetPointerCoordinate(bus.StopIndex);
        Vector3 stopCoordinate = _busStop.GetStopCoordinate(bus.StopIndex);

        bus.AssignBusStopPoints(pointerCoordinate, stopCoordinate);
    }

    private void UpdateColorHandle()
    {

    }
}
