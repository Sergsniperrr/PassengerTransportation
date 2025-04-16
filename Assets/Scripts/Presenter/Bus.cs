using System;
using UnityEngine;

[RequireComponent(typeof(BusRouter))]
[RequireComponent(typeof(ColorSetter))]
[RequireComponent(typeof(Loader))]
public class Bus : MonoBehaviour
{
    [SerializeField] private Vector3 _sizeAtStop;

    private readonly int _halfDevider = 2;
    private BusRouter _router;
    private Roof _roof;
    private ColorSetter _color;
    private Loader _loader;

    public event Action<Bus, int> StopReleased;
    public event Action<Bus, int> ArrivedToStop;

    public bool IsActive { get; private set; } = true;
    public int StopIndex { get; private set; }
    public int SeatsCount => _loader.Count;
    public Material Material => _color.Material;
    public bool IsEmptySeat => _loader.IsEmptySeat;

    private void Awake()
    {
        _router = GetComponent<BusRouter>();
        _roof = GetComponentInChildren<Roof>();
        _color = GetComponentInChildren<ColorSetter>();
        _loader = GetComponent<Loader>();

        if (_roof == null)
            throw new NullReferenceException(nameof(_roof));
    }

    private void OnDestroy()
    {
        Finish();
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

    private void EndMove()
    {
        _router.MoveCompleted -= EndMove;

        StopReleased?.Invoke(this, StopIndex);

        IsActive = true;
    }

    private void WaitPassengers()
    {
        transform.localScale = _sizeAtStop;
        _roof.gameObject.SetActive(false);

        _router.StopArrived -= WaitPassengers;

        _router.StopTriggerArrived += LeaveBusStop;

        ArrivedToStop?.Invoke(this, StopIndex);
    }

    private void LeaveBusStop()
    {
        _router.StopTriggerArrived -= LeaveBusStop;
        StopReleased?.Invoke(this, StopIndex);
    }

    private void Finish() =>
        StopReleased?.Invoke(this, StopIndex);

    private void GrowToHalfSizeAtStop()
    {
        _router.StopTriggerArrived -= GrowToHalfSizeAtStop;

        Vector3 scale = transform.localScale;
        transform.localScale = (_sizeAtStop - scale) /_halfDevider + scale;
    }
}
