using System;
using UnityEngine;

public class BusPointsCalculator : MonoBehaviour
{
    [SerializeField] private Transform[] _points;

    private readonly float _pointerShiftOnX = 1.65f;
    private readonly float _pointerShiftOnZ = 2.82f;
    private readonly float _finishPositionX = -36.5f;

    private Vector3 _position = Vector3.zero;

    public BusPoints CalculatePoints(int stopIndex, float positionY)
    {
        BusPoints points = new();
        Vector3 finish;

        points.SetStopPointer(CalculatePointerCoordinate(stopIndex, positionY));
        points.SetStop(CalculateStopCoordinate(stopIndex, positionY));

        finish = points.StopPointer;
        finish.x = _finishPositionX;
        points.SetFinish(finish);

        return points;
    }

    private Vector3 CalculatePointerCoordinate(int stopIndex, float positionY)
    {
        if (stopIndex < 0 || stopIndex >= _points.Length)
            throw new ArgumentOutOfRangeException(nameof(stopIndex));

        Vector3 position = Vector3.zero;

        position.x = _points[stopIndex].position.x + _pointerShiftOnX;
        position.z = _points[stopIndex].position.z + _pointerShiftOnZ;
        position.y = positionY;

        return position;
    }

    private Vector3 CalculateStopCoordinate(int stopIndex, float positionY)
    {
        if (stopIndex < 0 || stopIndex >= _points.Length)
            throw new ArgumentOutOfRangeException(nameof(stopIndex));

        Vector3 position = _points[stopIndex].position;
        position.y = positionY;

        return position;
    }
}
