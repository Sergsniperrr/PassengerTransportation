using System;
using Scripts.Presenters;
using UnityEngine;

[RequireComponent(typeof(PassengerPool))]
public class PassengerSpawner : MonoBehaviour
{
    private PassengerPool _pool;
    private Passenger _passenger;
    private IColorGetter _colors;

    private void Awake()
    {
        _pool = GetComponent<PassengerPool>();
    }

    public void InitializeColors(IColorGetter colors) =>
        _colors = colors ?? throw new NullReferenceException(nameof(colors));

    public Passenger Spawn()
    {
        if (_colors.ColorsCount == 0)
            return null;

        _passenger = _pool.GetObject();
        _passenger.SetColor(_colors.DequeuePassengerColor());
        _passenger.Died += Remove;

        return _passenger;
    }

    public void Remove(Passenger passenger)
    {
        _passenger.Died -= Remove;

        _pool.PutObject(passenger);
    }
}
