using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BusRouter))]
[RequireComponent(typeof(ColorSetter))]
[RequireComponent(typeof(Loader))]
[RequireComponent(typeof(TriggerHandler))]
[RequireComponent(typeof(MeshRenderer))]
public class Bus : MonoBehaviour, ISenderOfFillingCompletion
{
    private readonly WaitForSeconds _waitOfCheckPassengers = new(0.01f);

    private BusRouter _router;
    private Roof _roof;
    private ColorSetter _color;
    private Loader _loader;
    private TriggerHandler _trigger;

    public event Action<Bus> FillingCompleted;

    public bool IsActive => _router.IsActive;
    public int StopIndex => _router.StopIndex;
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

    public void InitializeData(IBusReceiver busStop, BusPointsCalculator navigator) =>
        _router.InitializeData(busStop, navigator);

    public void SetColor(Material material) =>
        _color.SetMateral(material);

    public void Run() =>
        _router.StartMove();

    public int ReserveFreePlace() =>
        _loader.ReserveFreePlace();

    public void TakePassenger(Passenger passenger) =>
        _loader.TakePassenger(passenger);

    public void MoveOutFromBusStop()
    {
        _router.MoveOutFromBusStop();

        _trigger.WayFinished += Finish;
    }

    public void CompleteFilling()
    {
        FillingCompleted?.Invoke(this);
    }

    private void Finish()
    {
        _trigger.WayFinished -= Finish;

        StartCoroutine(DestroyAfterCheckPassengers());
    }

    private void ReleasePassengers()
    {
        var passengers = GetComponentsInChildren<Passenger>();

        foreach (Passenger passenger in passengers)
            if (passenger != null)
                passenger.Finish();
            else
                throw new NullReferenceException(nameof(passenger));
    }

    private IEnumerator DestroyAfterCheckPassengers()
    {
        while (TryGetComponent(out Passenger _))
        {
            ReleasePassengers();

            yield return _waitOfCheckPassengers;
        }

        Destroy(gameObject);
    }
}
