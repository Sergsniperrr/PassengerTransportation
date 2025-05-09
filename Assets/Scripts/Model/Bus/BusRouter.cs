using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BusMover))]
[RequireComponent(typeof(TriggerHandler))]
public class BusRouter : MonoBehaviour
{
    private readonly float _finishCoordinateX = -50f;

    private BusMover _mover;
    private TriggerHandler _trigger;
    private Vector3 _initialCoordinate;
    private Vector3 _stopPointerCoordinate;
    private Vector3 _stopCoordinate;
    private WaitForSeconds _waitForStopToLeave = new(0.3f);

    public event Action MoveCompleted;
    public event Action StopArrived;
    public event Action StopTriggerArrived;

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
        MoveCompleted?.Invoke();

        _mover.GoBackwardsToPoint(_initialCoordinate);
        _trigger.DisableCrash();

        _mover.ArrivedAtPoint += StopMove;
    }

    public void MoveOutFromBusStop()
    {
        StartCoroutine(MoveOutFromBusStopWithDelay());
    }

    private IEnumerator MoveOutFromBusStopWithDelay()
    {
        yield return _waitForStopToLeave;

        _mover.GoToPoint(_stopPointerCoordinate);

        _mover.ArrivedAtPoint += MoveToFinish;
    }

    private void MoveToFinish()
    {
        _mover.ArrivedAtPoint -= MoveToFinish;

        Vector3 finishCoordinate = transform.position;
        finishCoordinate.x = _finishCoordinateX;
        StopTriggerArrived?.Invoke();

        _mover.GoToPoint(finishCoordinate);
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
        StopTriggerArrived?.Invoke();

        _mover.ArrivedAtPoint += GoToStop;
    }

    private void GoToStop()
    {
        _mover.ArrivedAtPoint -= GoToStop;

        _mover.GoToPoint(_stopCoordinate);

        _mover.ArrivedAtPoint += WaitForFilling;
    }

    private void WaitForFilling()
    {
        _mover.ArrivedAtPoint -= WaitForFilling;

        StopArrived?.Invoke();
    }
}
