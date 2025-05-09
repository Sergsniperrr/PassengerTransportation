using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QueueMover : MonoBehaviour
{
    private readonly int _rotaryIndex = 10;
    private readonly float _stepSize = 0.5f;
    private readonly float _reverseDerection = -1f;
    private readonly float _outPointShiftOnZ = 0.7f;

    private Vector3[] _coordinates;
    private Vector3 _outPoint;

    public void InitializeData(Vector3 zeroPosition, int elementsCount)
    {
        Vector3 shift = Vector3.zero;
        List<Vector3> coordinates = new();

        for (int i = 0; i < _rotaryIndex; i++)
        {
            shift.z = i * _stepSize;
            coordinates.Add(zeroPosition + shift);
        }

        for (int i = _rotaryIndex; i < elementsCount; i++)
        {
            shift.x = (i - _rotaryIndex + 1) * _stepSize * _reverseDerection;
            coordinates.Add(zeroPosition + shift);
        }

        coordinates.Reverse();
        _coordinates = coordinates.ToArray();

        CalculateOutPoint();
    }

    public void UpdatePositions(List<Passenger> queue)
    {
        for (int i = 0; i < queue.Count; i++)
            queue[i].MoveToNextPlaceInQueue();
    }

    public void StartMovePassenger(Passenger passenger, int index)
    {
        int minPositionZIndex = 15;

        if (index < minPositionZIndex)
            passenger.MoveTo(_coordinates[minPositionZIndex]);

        passenger.MoveTo(_coordinates[index]);
    }

    private void CalculateOutPoint()
    {
        _outPoint = _coordinates[0];
        _outPoint.z += _outPointShiftOnZ;
    }
}
