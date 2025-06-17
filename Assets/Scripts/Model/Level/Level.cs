using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MouseInputHandler))]
[RequireComponent(typeof(Colors))]
[RequireComponent(typeof(ColorsHandler))]
public class Level : MonoBehaviour
{
    [SerializeField] private BusStop _busStop;
    [SerializeField] private BusPointsCalculator _busNavigator;
    [SerializeField] private PassengerQueue _queue;
    [SerializeField] private Train _train;
    [SerializeField] private Effects _effects;
    [SerializeField] private PassengerCounter _passengerCounter;
    [SerializeField] private GameButtons _gameButtons;
    [SerializeField] private GameMenu _gameMenu;
    [SerializeField] private Music _music;

    private Colors _colors;
    private ColorsHandler _colorsHandler;
    private List<Bus> _buses;
    private MouseInputHandler _input;
    private bool _canBusMove;

    public List<Bus> Buses => new(_buses);

    private void Awake()
    {
        _input = GetComponent<MouseInputHandler>();
        _colors = GetComponent<Colors>();
        _colorsHandler = GetComponent<ColorsHandler>();
        InitializeBuses();
    }

    private void OnEnable()
    {
        _input.BusSelected += RunBus;
        _busStop.BusReceived += RemoveBus;
        _train.ArrivedAtStation += ActivatePassengers;
        _train.LeftStation += LevelComplete;
        _busStop.PassengerLeft += _passengerCounter.DecrementValue;
        _gameMenu.GameActiveChanged += ChangeGameActivity;
    }

    private void OnDisable()
    {
        _input.BusSelected -= RunBus;
        _busStop.BusReceived -= RemoveBus;
        _train.ArrivedAtStation -= ActivatePassengers;
        _train.LeftStation -= LevelComplete;
        _busStop.PassengerLeft -= _passengerCounter.DecrementValue;
        _gameMenu.GameActiveChanged -= ChangeGameActivity;
    }

    private void Start()
    {
        SetBusesRandomColor();
        _colorsHandler.InitializePassengerColors();
        _queue.InitializeColorsSpawner(_colorsHandler);

        _train.MoveToStation();
        _passengerCounter.SetValue(_colorsHandler.ColorsCount);

        _queue.PassengersCreated += ActivatePlayerInput;
    }

    public void SetBusesRandomColor()
    {
        foreach (Bus bus in _buses)
        {
            InitializeBusData(bus);
            bus.SetColor(_colors.GetRandomColor());
        }
    }

    private void InitializeBuses()
    {
        _buses = GetComponentsInChildren<Bus>().ToList();
    }

    private void RemoveBus(Bus bus)
    {
        if (bus != null)
            _buses.Remove(bus);

        if (_buses.Count == 1)
            _buses[0].Removed += SendOutTrain;
    }

    private void SendOutTrain(Bus lastBus)
    {
        lastBus.Removed -= SendOutTrain;

        _train.LeaveStation();
        _music.Stop();
        _effects.PlayLevelComplete();
    }

    private void InitializeBusData(Bus bus) =>
        bus.InitializeData(_busStop, _busNavigator, _effects);

    private void RunBus(Bus bus)
    {
        if (_canBusMove)
            bus.Run();
    }

    private void ActivatePassengers()
    {
        _queue.Spawn();
    }

    private void ActivatePlayerInput()
    {
        _queue.PassengersCreated -= ActivatePlayerInput;

        ChangeGameActivity(true);
    }

    private void ChangeGameActivity(bool isActive)
    {
        _canBusMove = isActive;
        _gameButtons.SetActive(isActive);
    }

    private void LevelComplete()
    {
        
    }
}
