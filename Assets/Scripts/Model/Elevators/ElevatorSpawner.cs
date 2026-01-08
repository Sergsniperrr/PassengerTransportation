using System;
using System.Collections.Generic;
using Scripts.Presenter;
using UnityEngine;

public class ElevatorSpawner : MonoBehaviour
{
    [SerializeField] private Bus _busTest1;
    [SerializeField] private Bus _busTest2;
    [SerializeField] private Bus _busTest3;
    [SerializeField] private Elevator _horizontalPrefab;
    [SerializeField] private Elevator _verticalPrefab;
    [SerializeField] private Elevator _diagonalLeftPrefab;
    [SerializeField] private Elevator _diagonalRightPrefab;

    private const float MaxAngle = 180f;
    private const float DiagonalRightAngle = 45f;
    private const float HorizontalAngle = 90f;
    private const float DiagonalLeftAngle = 135f;

    private Dictionary<float, Elevator> _prefabs;

    private void Awake()
    {
        _prefabs = new()
        {
            { DiagonalRightAngle, _diagonalRightPrefab },
            { HorizontalAngle, _horizontalPrefab },
            { DiagonalLeftAngle, _diagonalLeftPrefab },
            { MaxAngle, _verticalPrefab }
        };
    }

    public Elevator Spawn(BusData bus)
    {
        float angle = bus.Rotation.eulerAngles.y;

        angle = angle == 0 || angle == MaxAngle ? MaxAngle : angle < MaxAngle ? angle : angle - MaxAngle;
        angle = (float)Math.Round(angle);

        if (_prefabs.ContainsKey(angle) == false)
            throw new ArgumentOutOfRangeException(nameof(angle));

        Elevator elevator = Instantiate(_prefabs[angle]);
        elevator.transform.SetParent(transform);
        elevator.SetPosition(bus.Position);
        elevator.InitializeData(bus.Position, bus.Rotation);

        return elevator;
    }
}
