using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Scripts.Presenters;

[RequireComponent(typeof(QueueMover))]
[RequireComponent(typeof(PassengerSpawner))]
public class PassengerQueue : MonoBehaviour
{
    private int _maxVisibleQueueSize = 25;
    private List<Passenger> _queue = new();
    private QueueMover _mover;
    private PassengerSpawner _spawner;
    private WaitForSeconds _delayOfSpawnPassenger = new(0.07f);
    private Passenger _passenger;
    private float _changeLastPassengerDelay = 0.06f;
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
            _changeLastPassengerCounter = _changeLastPassengerDelay;
            _isNeedUpdateLastPassenger = false;

            LastPassengerChanged.Invoke();
        }

        _changeLastPassengerCounter -= Time.deltaTime;
    }

    public void InitializeColorsSpawner(IColorGetter colors)
    {
        _spawner.InitializeColors(colors);
    }

    public void Enqueue()
    {
        _passenger = _spawner.Spawn();

        if (_passenger == null || _queue.Count == _maxVisibleQueueSize)
            return;

        _passenger.InitializeQueueSize(_maxVisibleQueueSize);
        _queue.Add(_passenger);
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

    private IEnumerator InitialSpawnWithDelay()
    {
        Passenger passenger;

        for (int i = 0; i < _maxVisibleQueueSize; i++)
        {
            passenger = _spawner.Spawn();

            if (passenger != null)
            {
                passenger.InitializeQueueSize(_maxVisibleQueueSize);
                _queue.Add(passenger);
                _mover.StartMovePassengers(_queue);
            }

            yield return _delayOfSpawnPassenger;
        }

        _isNeedUpdateLastPassenger = true;
        PassengersCreated?.Invoke();
    }
}
