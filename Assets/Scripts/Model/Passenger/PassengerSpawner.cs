using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PassengerPool))]
public class PassengerSpawner : MonoBehaviour
{
    private PassengerPool _pool;
    private Passenger _passenger;
    private IColorGetter _colors;
    private Queue<Vector3> _coordinatesOfQueue = new();

    private void Awake()
    {
        _pool = GetComponent<PassengerPool>();
        CalculateQueuePositions();
    }

    public void InitializeColors(IColorGetter colors) =>
        _colors = colors ?? throw new NullReferenceException(nameof(colors));

    public Passenger Spawn()
    {
        if (_colors.ColorsCount == 0)
            return null;

        _passenger = _pool.GetObject();
        _passenger.InitialPositionsOfQueue(InitialPositionsInQueue());
        _passenger.SetColor(_colors.DequeuePassengerColor());
        _passenger.Died += Remove;

        return _passenger;
    }

    public void Remove(Passenger passenger)
    {
        _passenger.Died -= Remove;

        _pool.PutObject(passenger);
    }

    private Queue<Vector3> InitialPositionsInQueue()
    {
        return new Queue<Vector3>(_coordinatesOfQueue);
    }

    public void CalculateQueuePositions()
    {
        Vector3 pozition = Vector3.zero;
        int rotaryIndex = 10;
        float stepSize = 0.5f;
        int count = 25;

        for (int i = 1; i < count; i++)
        {
            pozition.z = Mathf.Min(i, rotaryIndex - 1) * stepSize;
            pozition.x = Mathf.Max(0, i - rotaryIndex + 1) * stepSize;
            pozition.x *= -1;

            _coordinatesOfQueue.Enqueue(transform.position + pozition);
        }
    }
}
