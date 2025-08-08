using System;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(MouseInputHandler))]
[RequireComponent(typeof(ColorsHandler))]
public class Level : MonoBehaviour
{
    [SerializeField] private BusStop _busStop;
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

    private ColorsHandler _colorsHandler;
    private List<Bus> _buses;
    private UndergroundBuses _undergroundBuses;
    private MouseInputHandler _input;
    private Music _menuMusic;
    private Coroutine _coroutine;
    private int _currentLevel;
    private bool _canBusMove;

    public event Action GameStarted;
    public event Action<Queue<Bus>> GameActivated;
    public event Action Completed;

    public List<Bus> Buses => new(_buses);

    private void Awake()
    {
        _input = GetComponent<MouseInputHandler>();

        _colorsHandler = GetComponent<ColorsHandler>();
    }

    private void OnEnable()
    {
        _input.BusSelected += RunBus;
        //_busStop.BusReceived += RemoveBus;
        _train.ArrivedAtStation += ActivatePassengers;
        _busStop.PassengerLeft += _passengerCounter.DecrementValue;
    }

    private void OnDisable()
    {
        _input.BusSelected -= RunBus;
        //_busStop.BusReceived -= RemoveBus;
        _train.ArrivedAtStation -= ActivatePassengers;
        _busStop.PassengerLeft -= _passengerCounter.DecrementValue;
    }

    public void Begin(int levelNumber, Music music, List<Bus> buses, UndergroundBuses undergroundBuses)
    {
        _currentLevel = levelNumber;
        _textLevelNomber.text = $"{levelNumber}";
        _menuMusic = music;
        _buses = buses ?? throw new ArgumentNullException(nameof(buses));

        _undergroundBuses = undergroundBuses != null ?
            undergroundBuses : throw new ArgumentNullException(nameof(undergroundBuses));

        _statisticsView.InitializeData(levelNumber, _buses.Count + undergroundBuses.Count);

        _windows.OpenBeginLevel().Closed += PlayGame;
    }

    public void ChangeGameActivity(bool isActive)
    {
        _canBusMove = isActive;
        _gameButtons.SetActive(isActive);
    }

    public void AddUndergroundBus(Bus bus)
    {
        if (bus == null)
            throw new ArgumentNullException(nameof(bus));

        _buses.Add(bus);
    }

    public void RemoveBus(Bus bus)
    {
        if (bus != null)
            _buses.Remove(bus);

        Debug.Log($"{_undergroundBuses.Count}; {_buses.Count}; {_coroutine == null}");

        if (_undergroundBuses.Count == 0 && _buses.Count == 0 && _coroutine == null)
            _coroutine = StartCoroutine(CheckBusCountForZero());
    }

    private void PlayGame(SimpleWindow window)
    {
        window.Closed -= PlayGame;

        _colorsHandler.InitializePassengerColors(_buses.ToArray(), _undergroundBuses.Buses);
        _queue.InitializeColorsSpawner(_colorsHandler);
        _train.MoveToStation();
        _passengerCounter.SetValue(_colorsHandler.ColorsCount);
        _menuMusic.Stop();
        _music.Play();

        GameStarted?.Invoke();
        _queue.PassengersCreated += ActivatePlayerInput;
        _busStop.AllPlacesOccupied += OpenDialog;
    }

    private void Complete()
    {
        _busStop.AllPlacesReleased -= Complete;

        float delay = 2f;

        _train.LeaveStation();
        _music.Stop();
        _effects.PlayLevelComplete();
        _windows.OpenLevelComplete(delay).Closed += Complete;
        _gameButtons.SetActive(false);
    }

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

        _gameButtons.SetActive(true);
    }

    private IEnumerator CheckBusCountForZero()
    {
        WaitForSeconds wait = new(0.2f);

        yield return wait;

        Debug.Log($"Underground buses: {_undergroundBuses.Count}; buses: {_buses.Count}");

        if (_undergroundBuses.Count == 0 && _buses.Count == 0)
            _busStop.AllPlacesReleased += Complete;

        _coroutine = null;
    }
}
