using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(QueueMover))]
[RequireComponent(typeof(PassengerSpawner))]
public class PassengerQueue : MonoBehaviour
{
    private int _indexOfpassengerBeforeLast = 1;
    private int _visibleQueueSize = 25;
    private List<Passenger> _queue = new();
    private QueueMover _mover;
    private PassengerSpawner _spawner;
    private Passenger _bufferForEnqueue;
    private Passenger _bufferForDequeue;
    private Passenger _bufferForShift;
    private WaitForSeconds _delayBeforeShift = new(0.03f);
    private WaitForSeconds _delayOfSpawnPassenger = new(0.07f);

    public Passenger LastPassenger { get; private set; }
    public int Count => _queue.Count;

    private void Awake()
    {
        _mover = GetComponent<QueueMover>();
        _spawner = GetComponent<PassengerSpawner>();
        _mover.InitializeData(_spawner.transform.position, _visibleQueueSize);
    }

    public void InitializeColorsSpawner(IColorGetter colors) =>
        _spawner.InitializeColors(colors);

    public Passenger GetPassengerByIndex(int index)
    {
        if (index < 0 || index >= _queue.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        return _queue[index];
    }

    public void Enqueue()
    {
        _bufferForEnqueue = _spawner.Spawn();

        if (_bufferForEnqueue == null || _queue.Count == _visibleQueueSize)
            return;

        StartCoroutine(EnqueueWithWaiting(_bufferForEnqueue));
    }

    public void Dequeue()
    {
        _queue.RemoveAt(0);
        LastPassenger = null;

        if (_queue.Count > 0)
        {
            if (_queue.Count > _indexOfpassengerBeforeLast)
                _queue[_indexOfpassengerBeforeLast].SpeedUp();

            _mover.UpdatePositions(_queue);
            LastPassenger = _queue[0];
        }

        Enqueue();
    }

    public Passenger ExtractPassenger(int index)
    {
        Passenger _buffer;

        if (index < 0 || index >= _queue.Count)
            throw new ArgumentOutOfRangeException(nameof(index));

        _buffer = _queue[index];

        return _buffer;
    }

    public void Spawn()
    {
        StartCoroutine(SpawnWithDelay());
    }

    private IEnumerator SpawnWithDelay()
    {
        for (int i = 0; i < _visibleQueueSize; i++)
        {
            _bufferForEnqueue = _spawner.Spawn();

            if (_bufferForEnqueue != null)
            {
                _queue.Add(_bufferForEnqueue);
                _bufferForEnqueue.SetPlaceIndex(i);
                _mover.StartMovePassenger(_queue[^1], _queue.Count - 1);

                _bufferForEnqueue.SkipPositionsOfQueue(_visibleQueueSize - i - 1);
            }

            yield return _delayOfSpawnPassenger;
        }

        LastPassenger = _queue[0];
        LastPassenger.SpeedUp();
    }

    private IEnumerator EnqueueWithWaiting(Passenger passenger)
    {
        yield return new WaitUntil(() => passenger.IsFinishedMovement);

        _queue.Add(passenger);
    }
}
