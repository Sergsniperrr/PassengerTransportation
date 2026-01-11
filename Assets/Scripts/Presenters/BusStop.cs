using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Model.BusStops;
using Scripts.Model.Level;
using UnityEngine;

namespace Scripts.Presenters
{
    [RequireComponent(typeof(BusPointsCalculator))]
    [RequireComponent(typeof(ColorAnalyzer))]
    public class BusStop : MonoBehaviour, IBusReceiver
    {
        private const int FailedIndex = -1;
        private const float DelayOfUpdateQueue = 0.06f;

        private readonly WaitForSeconds _waitForCheckGameOver = new(1.2f);
        private readonly WaitForSeconds _delayBeforeLeave = new(0.3f);
        private readonly Queue<Bus> _outGoingBuses = new();

        [SerializeField] private int _stopsCount = 7;
        [SerializeField] private int _initialFreeStopsCount = 7;
        [SerializeField] private PassengerQueue _queue;

        private ILevelCompleteable _levelCompleter;
        private ColorAnalyzer _colorAnalyzer;
        private Spot[] _spots;
        private Bus _outGoingBus;
        private Coroutine _coroutine;
        private float _updateCounter;
        private bool _canLeave = true;
        private bool _isFreePlace = true;

        public event Action<Bus> BusReceived;
        public event Action PassengerLeft;
        public event Action AllPlacesOccupied;
        public event Action<bool> PlacesVacateChanged;

        public bool IsAllPlacesReleased { get; private set; }

        private void Awake()
        {
            if (_initialFreeStopsCount > _stopsCount)
                throw new IndexOutOfRangeException(nameof(_initialFreeStopsCount));

            _colorAnalyzer = GetComponent<ColorAnalyzer>();

            _spots = new Spot[_initialFreeStopsCount];

            for (int i = 0; i < _initialFreeStopsCount; i++)
            {
                _spots[i] = new Spot(true);
            }
        }

        private void OnEnable()
        {
            _queue.LastPassengerChanged += HandlePassengersBoarding;
        }

        private void OnDisable()
        {
            _queue.LastPassengerChanged -= HandlePassengersBoarding;
        }

        private void Update()
        {
            if (_updateCounter > 0)
            {
                _updateCounter -= Time.deltaTime;
            }
            else
            {
                HandleBusesMoveOut();

                _updateCounter = DelayOfUpdateQueue;
            }
        }

        public void InitializeLevelCompleter(ILevelCompleteable levelCompleter) =>
            _levelCompleter = levelCompleter ?? throw new ArgumentNullException(nameof(levelCompleter));

        public int GetFreeStopIndex()
        {
            for (int i = 0; i < _stopsCount; i++)
            {
                if (_spots[i].IsFree)
                {
                    _spots[i].Reserve();
                    IsAllPlacesReleased = false;
                    return i;
                }
            }

            return FailedIndex;
        }

        public void ReleaseStop(int index)
        {
            if (index < 0 || index >= _spots.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            _spots[index].ReleaseStop();

            int occupiedPlaceCount = _spots.Count(spot => spot.BusAtBusStop != null || spot.BusOnWayToStop != null);

            if (occupiedPlaceCount == 0)
            {
                IsAllPlacesReleased = true;
                _levelCompleter.TryCompleteLevel();

                PlacesVacateChanged?.Invoke(true);
            }
        }

        public void AddBusOnWayToStop(Bus bus, int spotIndex)
        {
            if (spotIndex < 0 || spotIndex >= _stopsCount)
                throw new ArgumentOutOfRangeException(nameof(spotIndex));

            if (bus == null)
                throw new ArgumentNullException(nameof(bus));

            _spots[spotIndex].SetBusOnWayToStop(bus);
        }

        public void TakeBus(Bus bus, int platformIndex)
        {
            if (platformIndex < 0 || platformIndex >= _spots.Length)
                throw new ArgumentOutOfRangeException(nameof(platformIndex));

            _colorAnalyzer.AdFreePlaces(bus.Material, platformIndex, bus.SeatsCount);
            _spots[platformIndex].PlaceBusInStop();
            IsAllPlacesReleased = false;
            HandlePassengersBoarding();

            BusReceived?.Invoke(bus);

            PlacesVacateChanged?.Invoke(false);

            bus.FillingCompleted += AddToLeavingBusesQueue;
        }

        public void HandlePassengersBoarding()
        {
            if (_queue.LastPassenger != null && _queue.LastPassenger.IsFinishedMovement)
            {
                int stopIndex = _colorAnalyzer.GetPlatformOfDesiredColor(_queue.LastPassenger.Material);

                if (stopIndex != FailedIndex)
                {
                    _queue.LastPassenger.GotOnBus += GetOnBus;

                    _queue.LastPassenger.GetOnBus(_spots[stopIndex].BusAtBusStop);
                    _queue.RemoveLastPassenger();
                }
                else
                {
                    HandleGameOver();
                }
            }
        }

        public void ResetFreePlaces()
        {
            _isFreePlace = true;

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }

        public bool GetReservation(int spotIndex) =>
            _spots[spotIndex].IsFree;

        public Bus GetBus(int spotIndex)
        {
            if (_spots[spotIndex].BusOnWayToStop != null)
            {
                return _spots[spotIndex].BusOnWayToStop;
            }

            return _spots[spotIndex].BusAtBusStop;
        }

        private void GetOnBus(Passenger passenger)
        {
            passenger.GotOnBus -= GetOnBus;

            passenger.Bus.TakePassenger(passenger);
            PassengerLeft?.Invoke();
        }

        private void HandleBusesMoveOut()
        {
            if (_canLeave && _outGoingBuses.Count > 0)
            {
                _outGoingBus = _outGoingBuses.Dequeue();
                _outGoingBus.MoveOutFromBusStop();
                _canLeave = false;
                
                StartCoroutine(AllowLeave());
            }
        }

        private void AddToLeavingBusesQueue(Bus bus)
        {
            _outGoingBuses.Enqueue(bus);

            bus.FillingCompleted -= AddToLeavingBusesQueue;
        }

        private IEnumerator AllowLeave()
        {
            yield return _delayBeforeLeave;

            _canLeave = true;
        }

        private void HandleGameOver()
        {
            if (_isFreePlace == false)
                return;

            if (_spots.Any(spot => spot.BusAtBusStop == null))
                return;

            _isFreePlace = false;

            _coroutine = StartCoroutine(HandleGameOverAfterDelay());
        }

        private IEnumerator HandleGameOverAfterDelay()
        {
            yield return _waitForCheckGameOver;

            if (_spots.Any(spot => spot.BusAtBusStop == null) == false &&
                _colorAnalyzer.CheckDesiredColor(_queue.LastPassenger?.Material, _spots) == false)
            {
                AllPlacesOccupied?.Invoke();
            }
            else
            {
                ResetFreePlaces();
            }
        }
    }
}
