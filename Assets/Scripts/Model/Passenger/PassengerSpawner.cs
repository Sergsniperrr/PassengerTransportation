using System;
using System.Collections.Generic;
using UnityEngine;

public class PassengerSpawner : MonoBehaviour
{
    [SerializeField] private Passenger _prefab;

    private Passenger _passenger;
    private IColorGetter _colors;

    public void InitializeColors(IColorGetter colors) =>
        _colors = colors ?? throw new NullReferenceException(nameof(colors));

    public Passenger Spawn()
    {
        if (_colors.ColorsCount == 0)
            return null;

        _passenger = Instantiate(_prefab, transform.position, Quaternion.identity);
        _passenger.transform.SetParent(transform);
        _passenger.SetColor(_colors.DequeuePassengerColor());

        return _passenger;
    }
}
