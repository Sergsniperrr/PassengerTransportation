using System;
using UnityEngine;

public class BusStopNavigator : MonoBehaviour
{
    [SerializeField] private Transform[] _points;

    private readonly float _pointerShiftOnX = 1.65f;
    private readonly float _pointerShiftOnZ = 2.82f;

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
}
