using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BusRouter))]
[RequireComponent(typeof(ColorSetter))]
[RequireComponent(typeof(Loader))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BusView))]
public class Bus : MonoBehaviour, ISenderOfFillingCompletion
{
    private readonly WaitForSeconds _waitOfCheckPassengers = new(0.01f);

    private BusRouter _router;
    private Roof _roof;
    private ColorSetter _color;
    private Loader _loader;
    private BusView _transformChanger;

    public event Action<Bus> FillingCompleted;
    public event Action<Bus> Removed;

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
        _transformChanger = GetComponent<BusView>();

        if (_roof == null)
            throw new NullReferenceException(nameof(_roof));
    }

    public void InitializeData(IBusReceiver busStop, BusPointsCalculator navigator, ParticleSystem sparks) =>
        _router.InitializeData(busStop, navigator, sparks);

    public void SetColor(Material material) =>
        _color.SetMateral(material);

    public void Run() =>
        _router.StartMove();

    public int ReserveFreePlace() =>
        _loader.ReserveFreePlace();

    public void TakePassenger(Passenger passenger)
    {
        _loader.TakePassenger(passenger);
        _transformChanger.PulsateSize();
    }

    public void MoveOutFromBusStop()
    {
        _router.MoveOutFromBusStop();

        _router.WayFinished += Finish;
    }

    public void CompleteFilling()
    {
        FillingCompleted?.Invoke(this);
    }

    private void Finish()
    {
        _router.WayFinished -= Finish;

        StartCoroutine(DestroyAfterCheckPassengers());

        Removed?.Invoke(this);
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
