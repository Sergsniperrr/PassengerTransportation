using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseInputHandler))]
[RequireComponent(typeof(Colors))]
[RequireComponent(typeof(ColorsHandler))]
public class Level : MonoBehaviour
{
    [SerializeField] private BusStop _busStop;
    [SerializeField] private BusPointsCalculator _busNavigator;
    [SerializeField] private PassengerQueue _queue;

    private Colors _colors;
    private ColorsHandler _colorsHandler;
    private Bus[] _buses;
    private MouseInputHandler _input;

    private void Awake()
    {
        _input = GetComponent<MouseInputHandler>();
        _colors = GetComponent<Colors>();
        _colorsHandler = GetComponent<ColorsHandler>();
        _buses = GetComponentsInChildren<Bus>();
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
        SetBusesRandomColor();
        _colorsHandler.InitializeColors();
        _queue.InitializeColorsSpawner(_colorsHandler);
        _queue.Spawn();
    }

    public void SetBusesRandomColor()
    {
        foreach (Bus bus in _buses)
        {
            InitializeBusData(bus);
            bus.SetColor(_colors.GetRandomColor());
        }
    }

    private void InitializeBusData(Bus bus) =>
        bus.InitializeData(_busStop, _busNavigator);

    private void RunBus(Bus bus) =>
        bus.Run();
}
