using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(QueuePositionsCalculator))]
public class QueueMover : MonoBehaviour
{
    private readonly float _outPointShiftOnZ = 0.7f;

    private Vector3[] _coordinates;
    private Vector3 _outPoint;
    private int _queueSize;

    //public void InitializeData(Vector3 zeroPosition, int elementsCount)
    //{
    //    Vector3 shift = Vector3.zero;
    //    List<Vector3> coordinates = new();

    //    for (int i = 0; i < _rotaryIndex; i++)
    //    {
    //        shift.z = i * _stepSize;
    //        coordinates.Add(zeroPosition + shift);
    //    }

    //    for (int i = _rotaryIndex; i < elementsCount; i++)
    //    {
    //        shift.x = (i - _rotaryIndex + 1) * _stepSize * _reverseDerection;
    //        coordinates.Add(zeroPosition + shift);
    //    }

    //    coordinates.Reverse();
    //    _coordinates = coordinates.ToArray();

    //    CalculateOutPoint();
    //}

    public void InitislQueueSize(int size) =>
        _queueSize = size;

    public void IncrementPositions(List<Passenger> queue)
    {
        foreach (Passenger passenger in queue)
            passenger.IncrementCurrentIndex();
    }

    public void StartMovePassengers(List<Passenger> passengers)
    {
        for (int i = 0; i < passengers.Count; i++)
        {
            passengers[i].SetPlaceIndex(passengers.Count - i - 1);
        }
    }
}
