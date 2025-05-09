using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PassengerRouter))]
[RequireComponent(typeof(ColorSetter))]
public class Passenger : MonoBehaviour
{
    private PassengerRouter _router;
    private ColorSetter _color;

    public event Action<Passenger> Died;

    public int BusPlaceIndex { get; private set; }
    public bool IsFinishedMovement { get; private set; } = true;
    public Material Material => _color.Material;

    private void Awake()
    {
        _router = GetComponent<PassengerRouter>();
        _color = GetComponentInChildren<ColorSetter>();
    }

    public void InitialPositionsOfQueue(Queue<Vector3> positions) =>
        _router.InitialPositionsOfQueue(positions);

    public void SkipPositionsOfQueue(int countPositions) =>
    _router.SkipPositionsOfQueue(countPositions);

    public void SetColor(Material material) =>
        _color.SetMateral(material);

    public void MoveTo(Vector3 target)
    {
        _router.MoveTo(target);

        _router.ArrivedAtPoint += AllowExitFromQueue;
    }

    public void UpdatePosition(Vector3 target)
    {
        if (transform.position != target)
        {
            _router.MoveTo(target);
            IsFinishedMovement = false;

            _router.ArrivedAtPoint += AllowExitFromQueue;
        }
    }

    public void Finish() =>
        Died?.Invoke(this);

    public void AllowMovement() =>
        IsFinishedMovement = true;

    public void SetPlaceIndex(int index) =>
        _router.SetPlaceIndex(index);

    public void MoveToNextPlaceInQueue() =>
        _router.MoveToNextPlaceInQueue();

    public void GetOnBus(Bus bus)
    {
        if (bus == null)
            throw new ArgumentNullException(nameof(bus));

        int failedIndex = -1;
        BusPlaceIndex = bus.ReserveFreePlace();

        if (BusPlaceIndex == failedIndex)
            return;

        _router.ApproachToBus(bus);

        _router.ArrivedToBus += TakeBus;
    }

    public void SpeedUp() =>
        _router.SpeedUp();

    public void ResetSpeed() =>
        _router.ResetSpeed();

    private void AllowExitFromQueue()
    {
        _router.ArrivedAtPoint -= AllowExitFromQueue;

        AllowMovement();
    }

    private void TakeBus(Bus bus)
    {
        _router.ArrivedToBus -= TakeBus;

        bus.TakePassenger(this);
    }
}
