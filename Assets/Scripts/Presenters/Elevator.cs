using System;
using Scripts.Model.Elevators;
using Scripts.Model.Elevators.Data;
using Scripts.Model.Levels;
using Scripts.View.Elevator;
using UnityEngine;

namespace Scripts.Presenters
{
    [RequireComponent(typeof(ElevatorMover))]
    [RequireComponent(typeof(PositionCalculator))]
    [RequireComponent(typeof(ElevatorView))]
    public class Elevator : MonoBehaviour
    {
        private ElevatorMover _mover;
        private ElevatorView _view;
        private PositionCalculator _calculator;
        private Bus _bus;

        private Vector3 _busPosition;
        private Quaternion _busRotation;

        public event Action<Elevator> Released;
        public event Action<Bus, Elevator> BusLifted;

        [field: SerializeField] public string Name { get; private set; }
        public BusCounter Counter { get; private set; }

        private void Awake()
        {
            _mover = GetComponent<ElevatorMover>();
            _view = GetComponent<ElevatorView>();
            _calculator = GetComponent<PositionCalculator>();
            Counter = GetComponentInChildren<BusCounter>();

            if (Counter == null)
                throw new NullReferenceException(nameof(Counter));
        }

        public void SetPosition(Vector3 busPosition)
        {
            _calculator.Calculate(busPosition);
        }

        public void InitializeData(Vector3 busPosition, Quaternion busRotation)
        {
            _busPosition = busPosition;
            _busRotation = busRotation;
        }

        public void SetBusesCounter(int busesCount)
        {
            if (busesCount < 0)
                throw new ArgumentOutOfRangeException(nameof(busesCount));

            Counter.SetCount(busesCount);
        }

        public BusData ReleaseBus(BusUnderground bus)
        {
            BusData data = new (bus.SeatsCount, _busPosition, _busRotation);

            Counter.Decrement();

            return data;
        }

        public void LiftBus(Bus bus, float delay = 0f)
        {
            _bus = bus != null ? bus : throw new ArgumentNullException(nameof(bus));

            _mover.LiftBus(bus, delay);

            _mover.BusLifted += ActivateBus;
            _bus.LeftParkingLot += ReleaseBus;
        }

        public void DetectFirstBus()
        {
            const float Delay = 0.5f;

            Platform platform = GetComponentInChildren<Platform>();

            if (platform == null)
                throw new NullReferenceException(nameof(platform));

            Bus bus = platform.InitializeFirstBus();

            if (bus != null)
                LiftBus(bus, Delay);
        }

        private void ReleaseBus(Bus _)
        {
            _bus.LeftParkingLot -= ReleaseBus;

            if (Counter.Count > 0)
                Released?.Invoke(this);
        }

        private void ActivateBus(Bus bus)
        {
            _mover.BusLifted -= ActivateBus;

            if (Counter.Count == 0)
                _view.Hide();

            BusLifted?.Invoke(bus, this);
        }
    }
}