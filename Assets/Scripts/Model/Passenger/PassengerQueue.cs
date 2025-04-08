using System;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(QueueMover))]
[RequireComponent(typeof(PassengerSpawner))]
public class PassengerQueue : MonoBehaviour
{
    private int _visibleQueueSize = 25;
    private Passenger[] _queue;
    private QueueMover _mover;
    private PassengerSpawner _spawner;
    private Passenger _bufer;
    private bool _canSpawn = true;

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

        _bufer = _spawner.Spawn();

        if (_bufer != null)
            _queue[index] = _bufer;
    }

    public Passenger Dequeue()
    {
        _bufer = _queue[^1];
        _mover.MoveOutPassenger(_bufer);
        ShiftAll(_queue.Length - 1);

        return _bufer;
    }

    public Passenger GetLast() =>
        _queue[^1];

    public Passenger ExtractPassenger(int index)
    {
        if (index < 0 || index >= _queue.Length)
            throw new ArgumentOutOfRangeException(nameof(index));

        _bufer = _queue[index];
        ShiftAll(index);

        return _bufer;
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
            _bufer = _queue[i - 1];
            _queue[i - 1] = null;
            _queue[i] = _bufer;
        }

        _mover.MoveOneStep(_queue.ToArray());
    }
}
