using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PassengerRouter))]
[RequireComponent(typeof(ColorSetter))]
public class Passenger : MonoBehaviour
{
    private PassengerRouter _router;
    private ColorSetter _color;

    public int PlaceIndex { get; private set; }
    public Material Material => _color.Material;

    private void Awake()
    {
        _router = GetComponent<PassengerRouter>();
        _color = GetComponentInChildren<ColorSetter>();
    }

    public void SetColor(Material material) =>
        _color.SetMateral(material);

    public void MoveTo(Vector3 target) =>
        _router.MoveTo(target);

    public void GetOnBus(Bus bus)
    {
        if (bus == null)
            throw new ArgumentNullException(nameof(bus));

        int failedIndex = -1;
        PlaceIndex = bus.ReserveFreePlace();

        if (PlaceIndex == failedIndex)
            return;

        _router.ApproachToBus(bus);

        _router.ArrivedToBus += TakeBus;
    }

    private void TakeBus(Bus bus)
    {
        _router.ArrivedToBus -= TakeBus;

        bus.TakePassenger(this);
    }
}
