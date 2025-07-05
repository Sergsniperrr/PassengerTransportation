using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BusMover))]
[RequireComponent(typeof(TriggerHandler))]
[RequireComponent(typeof(BusView))]
[RequireComponent(typeof(PointsHandler))]
[RequireComponent(typeof(DirectionHandler))]
public class BusRouter : MonoBehaviour
{
    private const int FailedIndex = -1;

    private readonly WaitForSeconds _waitForStopToLeave = new(0.3f);

    private BusMover _mover;
    private TriggerHandler _trigger;
    private IBusReceiver _busStop;
    private BusView _view;
    private PointsHandler _pointsHandler;
    private DirectionHandler _directionHandler;

    public event Action LeftParkingLot;
    public event Action WayFinished;

    public int StopIndex { get; private set; } = FailedIndex;
    public bool IsActive { get; private set; } = true;

    private void Awake()
    {
        _mover = GetComponent<BusMover>();
        _trigger = GetComponent<TriggerHandler>();
        _view = GetComponent<BusView>();
        _pointsHandler = GetComponent<PointsHandler>();
        _directionHandler = GetComponent<DirectionHandler>();
    }

    public void InitializeData(IBusReceiver busStop, BusPointsCalculator calculator, Effects effects)
    {
        _busStop = busStop ?? throw new ArgumentNullException(nameof(busStop));
        _view.InitializeData(effects);

        _pointsHandler.InitializeData(calculator);
    }

    public void StartMove()
    {
        StopIndex = _busStop.GetFreeStopIndex();

        if (StopIndex == FailedIndex)
            return;

        _mover.EnableMovement();
        IsActive = false;
        _view.EnableSmoke();

        _trigger.BusCrashed += BackToInitialPlace;
        _directionHandler.LeftParkingLot += InformThatLeftParkingLot;
        _directionHandler.BusStopArrived += GoToStopPointer;
    }

    private void InformThatLeftParkingLot()
    {
        _directionHandler.LeftParkingLot -= InformThatLeftParkingLot;

        LeftParkingLot?.Invoke();
    }

    public void SetActive()
    {
        _pointsHandler.ReturnedToInitialPlace -= SetActive;

        IsActive = true;
    }

    public void BackToInitialPlace()
    {
        _trigger.BusCrashed -= BackToInitialPlace;
        _directionHandler.BusStopArrived -= GoToStopPointer;

        _view.DisableSmoke();
        _mover.GoBackwardsToPoint();
        _busStop.ReleaseStop(StopIndex);

        _pointsHandler.ReturnedToInitialPlace += SetActive;
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
        _view.EnableSmoke();

        _pointsHandler.EndpointArrived += Finish;
    }

    private void GoToStopPointer()
    {
        _trigger.BusCrashed -= BackToInitialPlace;
        _directionHandler.BusStopArrived -= GoToStopPointer;

        _view.GrowToHalfSizeAtStop();
        _mover.InitializeData(StopIndex);

        _pointsHandler.ArrivedToBusStop += WaitForFilling;
    }

    private void WaitForFilling()
    {
        _pointsHandler.ArrivedToBusStop -= WaitForFilling;

        if (TryGetComponent(out Bus bus))
            _busStop.TakeBus(bus, StopIndex);
    }

    private void Finish()
    {
        _pointsHandler.EndpointArrived -= Finish;

        WayFinished?.Invoke();
    }
}