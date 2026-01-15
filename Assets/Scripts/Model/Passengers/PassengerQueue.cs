using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Presenters;
using Scripts.View.Color;
using UnityEngine;

namespace Scripts.Model.Passengers
{
    [RequireComponent(typeof(QueueMover))]
    [RequireComponent(typeof(PassengerSpawner))]
    public class PassengerQueue : MonoBehaviour
    {
        private const int MaxVisibleQueueSize = 25;
        private const float ChangeLastPassengerDelay = 0.06f;

        private readonly List<Passenger> _queue = new ();
        private readonly WaitForSeconds _delayOfSpawnPassenger = new (0.07f);

        private QueueMover _mover;
        private PassengerSpawner _spawner;
        private Passenger _passenger;
        private float _changeLastPassengerCounter;
        private bool _isNeedUpdateLastPassenger;

        public event Action PassengersCreated;
        public event Action LastPassengerChanged;

        public Passenger LastPassenger { get; private set; }
        public Passenger[] Passengers => _queue.ToArray();

        private void Awake()
        {
            _mover = GetComponent<QueueMover>();
            _spawner = GetComponent<PassengerSpawner>();
        }

        private void Update()
        {
            if (_isNeedUpdateLastPassenger == false)
                return;

            if (_changeLastPassengerCounter <= 0 && _queue.Count > 0)
            {
                LastPassenger = _queue[0];
                LastPassenger.SpeedUp();
                _changeLastPassengerCounter = ChangeLastPassengerDelay;
                _isNeedUpdateLastPassenger = false;

                LastPassengerChanged?.Invoke();
            }

            _changeLastPassengerCounter -= Time.deltaTime;
        }

        public void InitializeColorsSpawner(IColorGetter colors)
        {
            _spawner.InitializeColors(colors);
        }

        public void RemoveLastPassenger()
        {
            _queue.RemoveAt(0);
            LastPassenger = null;

            if (_queue.Count > 0)
            {
                _isNeedUpdateLastPassenger = true;
                _mover.IncrementPositions(_queue);
            }

            Enqueue();
        }

        public void Spawn()
        {
            StartCoroutine(InitialSpawnWithDelay());
        }

        private void Enqueue()
        {
            _passenger = _spawner.Spawn();

            if (_passenger == null || _queue.Count == MaxVisibleQueueSize)
                return;

            _passenger.InitializeQueueSize(MaxVisibleQueueSize);
            _queue.Add(_passenger);
        }

        private IEnumerator InitialSpawnWithDelay()
        {
            Passenger passenger;

            for (int i = 0; i < MaxVisibleQueueSize; i++)
            {
                passenger = _spawner.Spawn();

                if (passenger != null)
                {
                    passenger.InitializeQueueSize(MaxVisibleQueueSize);
                    _queue.Add(passenger);
                    _mover.StartMovePassengers(_queue);
                }

                yield return _delayOfSpawnPassenger;
            }

            _isNeedUpdateLastPassenger = true;
            PassengersCreated?.Invoke();
        }
    }
}