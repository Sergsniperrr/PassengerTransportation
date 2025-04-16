using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PassengerMover))]
public class PassengerRouter : MonoBehaviour
{
    private readonly float _shiftPositionZToPlatform = 0.94f;
    private readonly float _firstPlatformPositionX = -31.45f;
    private readonly float _platformIntervalX = 1.6489f;
    private readonly Vector3 _shiftToBusEnter = new(0.48f, 0f, 0.92f);

    private PassengerMover _mover;
    private Vector3 _bufferPosition = Vector3.zero;
    private Bus _bus;

    public event Action<Bus> ArrivedToBus;

    private void Awake()
    {
        _mover = GetComponent<PassengerMover>();
    }

    public void MoveTo(Vector3 target) =>
    _mover.MoveTo(target);

    public void ApproachToBus(Bus bus)
    {
        _bus = bus;

        GoToBusStop();
        GoToPlatform();
        GoToBus();
    }

    private void GoToBusStop()
    {
        Vector3 lastPositionInQueue = new(-28.2f, 0.46f, 4.5f);
        _bufferPosition = lastPositionInQueue;
        _bufferPosition.z += _shiftPositionZToPlatform;

        _mover.MoveTo(_bufferPosition);
    }

    private void GoToPlatform()
    {
        _bufferPosition.x =_platformIntervalX * _bus.StopIndex + _firstPlatformPositionX;

        _mover.MoveTo(_bufferPosition);
    }

    private void GoToBus()
    {
        _mover.FinishMove(_bufferPosition + _shiftToBusEnter);

        _mover.ArrivedToBus += GetOnBus;
    }

    private void GetOnBus()
    {
        _mover.ArrivedToBus -= GetOnBus;
        ArrivedToBus?.Invoke(_bus);
    }
}
