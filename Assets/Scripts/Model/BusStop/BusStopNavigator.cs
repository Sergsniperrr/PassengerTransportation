using System;
using UnityEngine;

public class BusStopNavigator : MonoBehaviour
{
    [SerializeField] private Transform[] _points;

    private readonly float _pointerShiftOnX = 1.65f;
    private readonly float _pointerShiftOnZ = 2.82f;
    private readonly float _passengerBoardingShiftX = -0.89f;
    private readonly float _passengerBoardingShiftZ = -1.94f;

    private Vector3 _position = Vector3.zero;

    public Vector3 GetPointerCoordinate(int stopIndex)
    {
        if (stopIndex < 0 || stopIndex >= _points.Length)
            throw new ArgumentOutOfRangeException(nameof(stopIndex));

        _position.x = _points[stopIndex].position.x + _pointerShiftOnX;
        _position.z = _points[stopIndex].position.z + _pointerShiftOnZ;

        return _position;
    }

    public Vector3 GetStopCoordinate(int stopIndex)
    {
        if (stopIndex < 0 || stopIndex >= _points.Length)
            throw new ArgumentOutOfRangeException(nameof(stopIndex));

        return _points[stopIndex].position;
    }

    public Vector3 GetBoardingCoordinate(int stopIndex)
    {
        if (stopIndex < 0 || stopIndex >= _points.Length)
            throw new ArgumentOutOfRangeException(nameof(stopIndex));

        Vector3 coordinate = GetStopCoordinate(stopIndex);
        coordinate.x += _passengerBoardingShiftX;
        coordinate.z += _passengerBoardingShiftZ;

        return coordinate;
    }
}
