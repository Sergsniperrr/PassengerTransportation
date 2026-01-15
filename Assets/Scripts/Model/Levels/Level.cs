using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Input;
using Scripts.Model.Coins;
using Scripts.Model.Money;
using Scripts.Model.Other;
using Scripts.Model.Passengers;
using Scripts.Presenters;
using Scripts.Sounds;
using Scripts.View.Color;
using Scripts.View.Menu;
using Scripts.View.Text;
using Scripts.View.Windows;
using TMPro;
using UnityEngine;

namespace Scripts.Model.Levels
{
    [RequireComponent(typeof(MouseInputHandler))]
    [RequireComponent(typeof(ColorsHandler))]
    public class Level : MonoBehaviour, ILevelCompleteable
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
        [SerializeField] private Prices _prices;

        private ColorsHandler _colorsHandler;
        private List<Bus> _buses;
        private UndergroundBuses _undergroundBuses;
        private MouseInputHandler _input;
        private Coroutine _coroutine;
        private int _currentLevel;
        private bool _canBusMove;

        public event Action Started;
        public event Action<Queue<Bus>> GameActivated;
        public event Action Completed;
        public event Action<int> BussesCountInitialized;
        public event Action AllBussesLeft;

        [field: SerializeField] public CoinsHandler Coins { get; private set; }
        public List<Bus> Buses => new (_buses);

        private void Awake()
        {
            _input = GetComponent<MouseInputHandler>();
            Coins.HideCounters();
            _colorsHandler = GetComponent<ColorsHandler>();
            _busStop.InitializeLevelCompleter(this);
        }

        private void OnEnable()
        {
            _input.BusSelected += RunBus;
            _train.ArrivedAtStation += ActivatePassengers;
            _busStop.PassengerLeft += _passengerCounter.DecrementValue;
        }

        private void OnDisable()
        {
            _input.BusSelected -= RunBus;
            _train.ArrivedAtStation -= ActivatePassengers;
            _busStop.PassengerLeft -= _passengerCounter.DecrementValue;
        }

        public void Begin(int levelNumber, List<Bus> buses, UndergroundBuses undergroundBuses)
        {
            _currentLevel = levelNumber;
            _textLevelNomber.text = $"{levelNumber}";
            _buses = buses ?? throw new ArgumentNullException(nameof(buses));

            _undergroundBuses = undergroundBuses != null
                ? undergroundBuses
                : throw new ArgumentNullException(nameof(undergroundBuses));

            _statisticsView.InitializeData(levelNumber, _buses.Count + undergroundBuses.Count);
            Coins.InitializeNewLevel(levelNumber, _buses.Count + undergroundBuses.Count);

            BussesCountInitialized?.Invoke(_buses.Count + undergroundBuses.Count);

            _windows.OpenBeginLevel().Closed += PlayGame;
        }

        public void ChangeGameActivity(bool isActive)
        {
            _canBusMove = isActive;
            _gameButtons.gameObject.SetActive(isActive);
        }

        public void AddUndergroundBus(Bus bus)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));

            _buses.Add(bus);
        }

        private void RunBus(Bus bus)
        {
            if (_canBusMove)
                bus.Run();
        }

        private void PlayGame(SimpleWindow window)
        {
            window.Closed -= PlayGame;

            _colorsHandler.InitializePassengerColors(_buses.ToArray(), _undergroundBuses.Buses);
            _queue.InitializeColorsSpawner(_colorsHandler);
            _train.MoveToStation();
            _passengerCounter.SetValue(_colorsHandler.ColorsCount);
            _music.Play();

            Started?.Invoke();
            _queue.PassengersCreated += ActivatePlayerInput;
            _busStop.AllPlacesOccupied += HandleOccupiedSeatsEvent;
        }

        public void RemoveBus(Bus bus)
        {
            if (bus != null)
                _buses.Remove(bus);

            TryCompleteLevel();
        }

        public void TryCompleteLevel()
        {
            if (_coroutine == null && _busStop.IsAllPlacesReleased &&
                _undergroundBuses.Count == 0 && _buses.Count == 0)
            {
                _coroutine = StartCoroutine(CheckBusCountForZero());
            }
        }

        private void Complete()
        {
            const float Delay = -1f;

            AllBussesLeft?.Invoke();

            _coroutine = null;
            Coins.CompleteLevel();
            _train.LeaveStation();
            _music.Stop();
            _effects.PlayLevelComplete();

            LevelCompleteWindow window = (LevelCompleteWindow)_windows.OpenLevelComplete(Delay);
            window.InitializeCoins(Coins.TemporaryMoney, Coins.TemporaryScore);
            window.Closed += Complete;

            _gameButtons.gameObject.SetActive(false);
        }

        private void ActivatePassengers()
        {
            _queue.Spawn();
        }

        private void ActivatePlayerInput()
        {
            _queue.PassengersCreated -= ActivatePlayerInput;

            ChangeGameActivity(true);
            Coins.ShowCounters();

            GameActivated?.Invoke(new Queue<Bus>(_buses));
        }

        private void Complete(SimpleWindow window)
        {
            Coins.HideCounters();
            window.Closed -= Complete;
            _busStop.AllPlacesOccupied -= HandleOccupiedSeatsEvent;
            Completed?.Invoke();
        }

        private void HandleOccupiedSeatsEvent()
        {
            if (Coins.Money.Count < _prices.ArrangingPassengers)
                OpenDialog();
            else
                _gameButtons.PlayButtonPassengerArrangePulsation();
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

            _gameButtons.gameObject.SetActive(true);
        }

        private IEnumerator CheckBusCountForZero()
        {
            WaitForSeconds wait = new (0.2f);

            yield return wait;

            if (_undergroundBuses.Count == 0 && _buses.Count == 0 && _busStop.IsAllPlacesReleased)
                Complete();

            _coroutine = null;
        }
    }
}