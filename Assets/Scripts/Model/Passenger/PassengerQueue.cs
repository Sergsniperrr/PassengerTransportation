using System;
using UnityEngine;
using System.Linq;
using System.Collections;

[RequireComponent(typeof(QueueMover))]
[RequireComponent(typeof(PassengerSpawner))]
public class PassengerQueue : MonoBehaviour, IActivePassenger
{
    private int _visibleQueueSize = 25;
    private Passenger[] _queue;
    private QueueMover _mover;
    private PassengerSpawner _spawner;
    private Passenger _bufferForEnqueue;
    private Passenger _bufferForDequeue;
    private Passenger _bufferForShift;

    public event Action LastPassengerChanged;

    public Passenger LastPassenger => _queue[^1];

    private void Awake()
    {
        _mover = GetComponent<QueueMover>();
        _spawner = GetComponent<PassengerSpawner>();
        _queue = new Passenger[_visibleQueueSize];
        _mover.InitializeData(_spawner.transform.position, _visibleQueueSize);
    }

    public void InitializeColorsSpawner(IColorGetter colors) =>
        _spawner.InitializeColors(colors);

    public void Enqueue(int index = 0)
    {
        if (_queue[index] != null)
            throw new Exception("_queue[0] is not null value!");

        _bufferForEnqueue = _spawner.Spawn();

        if (_bufferForEnqueue != null)
            _queue[index] = _bufferForEnqueue;
    }

    public Passenger Dequeue()
    {
        _bufferForDequeue = _queue[^1];
        _mover.MoveOutPassenger(_bufferForDequeue);
        ShiftAll(_queue.Length - 1);

        return _bufferForDequeue;
    }

    public Passenger ExtractPassenger(int index)
    {
        Passenger _buffer;

        if (index < 0 || index >= _queue.Length)
            throw new ArgumentOutOfRangeException(nameof(index));

        _buffer = _queue[index];
        ShiftAll(index);

        return _buffer;
    }

    public void Spawn()
    {
        for (int i = _visibleQueueSize - 1; i >= 0; i--)
            Enqueue(i);

        _mover.StartMove(_queue);
    }

    private void ShiftAll(int freeElementIndex)
    {
        int minIndexForShift = 1;

        if (freeElementIndex < minIndexForShift || freeElementIndex >= _queue.Length)
            throw new ArgumentOutOfRangeException(nameof(freeElementIndex));


        for (int i = freeElementIndex; i > 0; i--)
        {
            _bufferForShift = _queue[i - 1];
            _queue[i - 1] = null;
            _queue[i] = _bufferForShift;
        }

        Enqueue();
        _mover.MoveOneStep(_queue.ToArray());
    }
}
