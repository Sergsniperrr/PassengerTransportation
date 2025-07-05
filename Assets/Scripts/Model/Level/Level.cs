using System;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

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
    [SerializeField] private WindowsHandler _windows;
    [SerializeField] private LevelStatisticsView _statisticsView;
    [SerializeField] private Music _music;
    [SerializeField] private TextMeshProUGUI _textLevelNomber;
    [SerializeField] private GameResetter _resetter;

    private Colors _colors;
    private ColorsHandler _colorsHandler;
    private List<Bus> _buses;
    private MouseInputHandler _input;
    private Music _menuMusic;
    private int _currentLevel;
    private bool _canBusMove;

    public event Action<Queue<Bus>> GameActivated;
    public event Action Completed;

    public List<Bus> Buses => new(_buses);

    private void Awake()
    {
        _input = GetComponent<MouseInputHandler>();
        _colors = GetComponent<Colors>();
        _colorsHandler = GetComponent<ColorsHandler>();
    }

    private void OnEnable()
    {
        _input.BusSelected += RunBus;
        _busStop.BusReceived += RemoveBus;
        _train.ArrivedAtStation += ActivatePassengers;
        _busStop.PassengerLeft += _passengerCounter.DecrementValue;
    }

    private void OnDisable()
    {
        _input.BusSelected -= RunBus;
        _busStop.BusReceived -= RemoveBus;
        _train.ArrivedAtStation -= ActivatePassengers;
        _busStop.PassengerLeft -= _passengerCounter.DecrementValue;
    }

    public void SetBusesRandomColor()
    {
        foreach (Bus bus in _buses)
        {
            InitializeBusData(bus);
            bus.SetColor(_colors.GetRandomColor());
        }
    }

    public void Begin(int levelNumber, Music music, List<Bus> buses)
    {
        _currentLevel = levelNumber;
        _textLevelNomber.text = $"{levelNumber + 1}";
        _menuMusic = music;
        _buses = buses ?? throw new ArgumentNullException(nameof(buses));
        _statisticsView.InitializeData(levelNumber, _buses.Count);
        _windows.OpenBeginLevel().Closed += PlayGame;
    }

    public void ChangeGameActivity(bool isActive)
    {
        _canBusMove = isActive;
        _gameButtons.SetActive(isActive);
    }

    private void PlayGame(SimpleWindow window)
    {
        window.Closed -= PlayGame;

        SetBusesRandomColor();
        _colorsHandler.InitializePassengerColors();
        _queue.InitializeColorsSpawner(_colorsHandler);
        _train.MoveToStation();
        _passengerCounter.SetValue(_colorsHandler.ColorsCount);
        _menuMusic.Stop();
        _music.Play();

        _queue.PassengersCreated += ActivatePlayerInput;
        _busStop.AllPlacesOccupied += OpenDialog;
    }

    private void RemoveBus(Bus bus)
    {
        if (bus != null)
            _buses.Remove(bus);

        if (_buses.Count == 1)
            _buses[0].Removed += Complete;
    }

    private void Complete(Bus lastBus)
    {
        lastBus.Removed -= Complete;

        float delay = 2f;

        _train.LeaveStation();
        _music.Stop();
        _effects.PlayLevelComplete();
        _windows.OpenLevelComplete(delay).Closed += Complete;
        _gameButtons.SetActive(false);
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

        GameActivated?.Invoke(new Queue<Bus>(_buses));
    }

    private void Complete(SimpleWindow window)
    {
        window.Closed -= Complete;
        _busStop.AllPlacesOccupied -= OpenDialog;
        Completed?.Invoke();
    }

    private void OpenDialog()
    {
        if (_buses.Count == 0)
            return;

        _windows.OpenWarningWindow();

        _windows.ResultResieved += HandleDialog;
    }

    private void HandleDialog(bool result)
    {
        _windows.ResultResieved -= HandleDialog;

        if (result == false)
            _resetter.BackInMainMenu(_currentLevel);
    }
}
