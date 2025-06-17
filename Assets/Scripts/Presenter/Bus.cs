using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BusRouter))]
[RequireComponent(typeof(ColorSetter))]
[RequireComponent(typeof(PassengerReception))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(BusView))]
public class Bus : MonoBehaviour, ISenderOfFillingCompletion
{
    private readonly WaitForSeconds _waitOfCheckPassengers = new(0.01f);

    private BusRouter _router;
    private Roof _roof;
    private ColorSetter _color;
    private PassengerReception _passengerReception;
    private BusView _transformChanger;
    private Effects _effects;

    public event Action<Bus> FillingCompleted;
    public event Action<Bus> Removed;

    public bool IsActive => _router.IsActive;
    public int StopIndex => _router.StopIndex;
    public int SeatsCount => _passengerReception.Count;
    public Material Material => _color.Material;
    public bool IsEmptySeat => _passengerReception.IsEmptySeat;
    public int FreeSeatsCount => _passengerReception.EmptySeatCount;

    private void Awake()
    {
        _router = GetComponent<BusRouter>();
        _roof = GetComponentInChildren<Roof>();
        _color = GetComponentInChildren<ColorSetter>();
        _passengerReception = GetComponent<PassengerReception>();
        _transformChanger = GetComponent<BusView>();

        if (_roof == null)
            throw new NullReferenceException(nameof(_roof));
    }

    public void InitializeData(IBusReceiver busStop, BusPointsCalculator navigator, Effects effects)
    {
        _router.InitializeData(busStop, navigator, effects);
        _effects = effects != null ? effects : throw new NullReferenceException(nameof(effects));
    }

    public void SetColor(Material material) =>
        _color.SetMateral(material);

    public void Run() =>
        _router.StartMove();

    public int ReserveFreePlace() =>
        _passengerReception.ReserveFreePlace();

    public void TakePassenger(Passenger passenger)
    {
        _passengerReception.TakePassenger(passenger);
        _transformChanger.BoardingEffect();
    }

    public void MoveOutFromBusStop()
    {
        _router.MoveOutFromBusStop();

        _router.WayFinished += Finish;
    }

    public void CompleteFilling()
    {
        _effects.PlayBusFillingComplete();

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
