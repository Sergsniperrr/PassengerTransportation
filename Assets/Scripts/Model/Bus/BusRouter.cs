using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BusMover))]
[RequireComponent(typeof(TriggerHandler))]
[RequireComponent(typeof(TransformChanger))]
[RequireComponent(typeof(PointsHandler))]
public class BusRouter : MonoBehaviour
{
    private const int FailedIndex = -1;

    private readonly WaitForSeconds _waitForStopToLeave = new(0.3f);

    private BusMover _mover;
    private TriggerHandler _trigger;
    private IBusReceiver _busStop;
    private TransformChanger _transformChanger;
    private PointsHandler _pointsHandler;

    public int StopIndex { get; private set; } = FailedIndex;
    public bool IsActive { get; private set; } = true;

    private void Awake()
    {
        _mover = GetComponent<BusMover>();
        _trigger = GetComponent<TriggerHandler>();
        _transformChanger = GetComponent<TransformChanger>();
        _pointsHandler = GetComponent<PointsHandler>();
    }

    public void InitializeData(IBusReceiver busStop, BusPointsCalculator calculator)
    {
        _busStop = busStop ?? throw new ArgumentNullException(nameof(busStop));

        _mover.InitializeBusStop(busStop);
        _pointsHandler.InitializeData(calculator);
    }

    public void StartMove()
    {
        StopIndex = _busStop.GetFreeStopIndex();

        if (StopIndex == FailedIndex)
            return;

        _mover.EnableMovement();
        _trigger.EnableCrash();
        IsActive = false;

        _trigger.BusCrashed += BackToInitialPlace;
        _trigger.BusStopTriggered += GoToStopPointer;
    }

    public void SetActive() =>
        IsActive = true;

    public void BackToInitialPlace()
    {
        _trigger.BusCrashed -= BackToInitialPlace;
        _trigger.BusStopTriggered -= GoToStopPointer;

        _mover.GoBackwardsToPoint();
        SetActive();
        _busStop.ReleaseStop(StopIndex);
    }

    public void MoveOutFromBusStop()
    {
        StartCoroutine(MoveOutFromBusStopWithDelay());
    }

    private IEnumerator MoveOutFromBusStopWithDelay()
    {
        yield return _waitForStopToLeave;

        _busStop.ReleaseStop(StopIndex);
        _mover.MoveOutFromBusStop();
    }

    private void GoToStopPointer()
    {
        _trigger.BusCrashed -= BackToInitialPlace;
        _trigger.BusStopTriggered -= GoToStopPointer;

        _transformChanger.GrowToHalfSizeAtStop();
        _mover.InitializeData(StopIndex);

        _pointsHandler.ArrivedToBusStop += WaitForFilling;
    }

    private void WaitForFilling()
    {
        _pointsHandler.ArrivedToBusStop -= WaitForFilling;

        if (TryGetComponent(out Bus bus))
            _busStop.TakeBus(bus, StopIndex);
    }
}