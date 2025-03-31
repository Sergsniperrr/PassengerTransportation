using System;
using UnityEngine;

[RequireComponent(typeof(BusMover))]
[RequireComponent(typeof(TriggerHandler))]
public class BusRouter : MonoBehaviour
{
    private BusMover _mover;
    private TriggerHandler _trigger;

    private Vector3 _initialCoordinate;
    private Vector3 _stopPointerCoordinate;
    private Vector3 _stopCoordinate;

    public event Action<bool> MoveComplited;
    public event Action StopArrived;

    private void Awake()
    {
        _mover = GetComponent<BusMover>();
        _trigger = GetComponent<TriggerHandler>();

        _initialCoordinate = transform.position;
    }

    public void StartMove()
    {
        _mover.Run();
        _trigger.EnableCrash();

        _trigger.BusCrashed += BackToInitialPlace;
        _trigger.BusStopTriggered += GoToStopPointer;
    }

    public void AssignBusStopPoints(Vector3 pointerCoordinate, Vector3 stopCoordinate)
    {
        _stopPointerCoordinate = pointerCoordinate;
        _stopPointerCoordinate.y = transform.position.y;

        _stopCoordinate = stopCoordinate;
        _stopCoordinate.y = transform.position.y;
    }

    public void BackToInitialPlace()
    {
        MoveComplited?.Invoke(false);

        _mover.GoBackwardsToPoint(_initialCoordinate);
        _trigger.DisableCrash();

        _mover.ArrivedAtPoint += StopMove;
    }

    private void StopMove()
    {
        _mover.ArrivedAtPoint -= StopMove;
        _mover.Stop();
    }

    private void GoToStopPointer()
    {
        _trigger.BusCrashed -= BackToInitialPlace;
        _trigger.BusStopTriggered -= GoToStopPointer;

        _mover.GoToPoint(_stopPointerCoordinate);

        _mover.ArrivedAtPoint += GoToStop;
    }

    private void GoToStop()
    {
        _mover.ArrivedAtPoint -= GoToStop;

        _mover.GoToPoint(_stopCoordinate);

        _mover.ArrivedAtPoint += WaitFill;
    }
    private void WaitFill()
    {
        _mover.ArrivedAtPoint -= WaitFill;
        StopArrived?.Invoke();
    }
}
