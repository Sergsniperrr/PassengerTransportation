using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BusRouter))]
[RequireComponent(typeof(ColorSetter))]
[RequireComponent(typeof(Loader))]
[RequireComponent(typeof(TriggerHandler))]
public class Bus : MonoBehaviour
{
    [SerializeField] private Vector3 _sizeAtStop;

    private const int FailedIndex = -1;

    private readonly int _halfDevider = 2;

    private BusRouter _router;
    private Roof _roof;
    private ColorSetter _color;
    private Loader _loader;
    private TriggerHandler _trigger;

    public event Action<Bus, int> StopReleased;
    public event Action<Bus, int> ArrivedToStop;
    public event Action<Bus> LoadCompleted;

    public bool IsActive { get; private set; } = true;
    public int StopIndex { get; private set; } = FailedIndex;
    public int SeatsCount => _loader.Count;
    public Material Material => _color.Material;
    public bool IsEmptySeat => _loader.IsEmptySeat;
    public int FreeSeatsCount => _loader.EmptySeatCount;

    private void Awake()
    {
        _router = GetComponent<BusRouter>();
        _roof = GetComponentInChildren<Roof>();
        _color = GetComponentInChildren<ColorSetter>();
        _loader = GetComponent<Loader>();
        _trigger = GetComponent<TriggerHandler>();

        if (_roof == null)
            throw new NullReferenceException(nameof(_roof));
    }

    public void SetColor(Material material) =>
        _color.SetMateral(material);

    public void Run()
    {
        _router.StartMove();
        IsActive = false;

        _router.MoveCompleted += EndMove;
        _router.StopArrived += WaitPassengers;
        _router.StopTriggerArrived += GrowToHalfSizeAtStop;
    }

    public void SetStopIndex(int index) =>
        StopIndex = index;

    public int ReserveFreePlace() =>
        _loader.ReserveFreePlace();

    public void TakePassenger(Passenger passenger) =>
        _loader.TakePassenger(passenger);

    public void AssignBusStopPoints(Vector3 pointerCoordinate, Vector3 stopCoordinate) =>
        _router.AssignBusStopPoints(pointerCoordinate, stopCoordinate);

    public void MoveOutFromBusStop() =>
        _router.MoveOutFromBusStop();

    public void ReleaseBusStop()
    {
        _router.StopTriggerArrived -= ReleaseBusStop;

        StopReleased?.Invoke(this, StopIndex);

        _trigger.WayFinished += Finish;
    }

    private void TryLeaveBusStop()
    {
        _loader.FillingCompleted -= TryLeaveBusStop;

        LoadCompleted?.Invoke(this);
    }

    private void EndMove()
    {
        _router.MoveCompleted -= EndMove;

        StopReleased?.Invoke(this, StopIndex);

        StopIndex = FailedIndex;
        IsActive = true;
    }

    private void WaitPassengers()
    {
        transform.localScale = _sizeAtStop;
        _roof.gameObject.SetActive(false);

        _router.StopArrived -= WaitPassengers;
        _router.StopTriggerArrived += ReleaseBusStop;

        ArrivedToStop?.Invoke(this, StopIndex);

        _loader.FillingCompleted += TryLeaveBusStop;
    }

    private void Finish()
    {
        _trigger.WayFinished -= Finish;

        for (int i = 0; i < SeatsCount; i++)
            _loader.GetPassengerByIndex(i).Finish();

        StartCoroutine(DestroyWithDelay());
    }

    private IEnumerator DestroyWithDelay()
    {
        WaitForSeconds wait = new(0.3f);

        yield return wait;

        Destroy(gameObject);
    }

    private void GrowToHalfSizeAtStop()
    {
        _router.StopTriggerArrived -= GrowToHalfSizeAtStop;

        Vector3 scale = transform.localScale;
        transform.localScale = (_sizeAtStop - scale) /_halfDevider + scale;
    }
}
