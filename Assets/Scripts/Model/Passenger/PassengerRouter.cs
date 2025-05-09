using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PassengerMover))]
public class PassengerRouter : MonoBehaviour
{
    private readonly float _shiftPositionZToPlatform = 0.94f;
    private readonly float _firstPlatformPositionX = -31.45f;
    private readonly float _platformIntervalX = 1.6489f;
    private readonly Vector3 _shiftToBusEnter = new(0.48f, 0f, 0.92f);
    private readonly Vector3 _lastPositionInQueue = new(-28.2f, 0.46f, 4.5f);
    private readonly Vector3 _busStopPosition = new(-28.2f, 0.46f, 5.44f);


    private PassengerMover _mover;
    private Vector3 _bufferPosition = Vector3.zero;
    private Bus _bus;

    public event Action ArrivedAtPoint;
    public event Action<Bus> ArrivedToBus;

    private void Awake()
    {
        _mover = GetComponent<PassengerMover>();
    }

    public void InitialPositionsOfQueue(Queue<Vector3> positions) =>
        _mover.InitialPositionsOfQueue(positions);

    public void SkipPositionsOfQueue(int countPositions) =>
        _mover.SkipPositionsOfQueue(countPositions);

    public void MoveTo(Vector3 target)
    {
        _mover.MoveTo(target);

        _mover.MoveCompleted += FinishMove;
    }

    public void ApproachToBus(Bus bus)
    {
        _bus = bus;

        GoToBusStop();
        GoToPlatform();
        GoToBus();
    }

    public void SpeedUp() =>
        _mover.SpeedUp();

    public void ResetSpeed() =>
        _mover.ResetSpeed();

    public void SetPlaceIndex(int index) =>
        _mover.SetPlaceIndex(index);

    public void MoveToNextPlaceInQueue() =>
        _mover.MoveToNextPlaceInQueue();

    private void GoToBusStop() =>
        _mover.MoveTo(_busStopPosition);

    private void FinishMove()
    {
        _mover.MoveCompleted -= FinishMove;

        ArrivedAtPoint?.Invoke();
    }

    private void GoToPlatform()
    {
        _bufferPosition = _busStopPosition;
        _bufferPosition.x = _platformIntervalX * _bus.StopIndex + _firstPlatformPositionX;

        _mover.MoveTo(_bufferPosition);
    }

    private void GoToBus()
    {
        _mover.MoveTo(_bufferPosition + _shiftToBusEnter);

        _mover.MoveCompleted += GetOnBus;
    }

    private void GetOnBus()
    {
        _mover.MoveCompleted -= GetOnBus;

        ArrivedToBus?.Invoke(_bus);
    }
}
