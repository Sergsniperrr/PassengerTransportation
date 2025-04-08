using System;
using UnityEngine;

[RequireComponent(typeof(BusRouter))]
[RequireComponent(typeof(Loader))]
[RequireComponent(typeof(Seats))]
[RequireComponent(typeof(ColorSetter))]
public class Bus : MonoBehaviour
{
    private BusRouter _router;
    private Loader _loader;
    private Roof _roof;
    private Seats _seats;
    private ColorSetter _color;

    public event Action<Bus, int> StopReleased;

    public bool IsActive { get; private set; } = true;
    public int StopIndex { get; private set; }
    public int SeatsCount => _seats.Count;
    public Material Material => _color.Material;

    private void Awake()
    {
        _router = GetComponent<BusRouter>();
        _loader = GetComponent<Loader>();
        _roof = GetComponentInChildren<Roof>();
        _seats = GetComponentInChildren<Seats>();
        _color = GetComponentInChildren<ColorSetter>();

        if (_roof == null)
            throw new NullReferenceException(nameof(_roof));
    }

    public void SetColor(Material material) =>
        _color.SetMateral(material);

    public void Run()
    {
        _router.StartMove();
        IsActive = false;

        _router.MoveComplited += EndMove;
        _router.StopArrived += RemoveRoof;
    }

    public void SetStopIndex(int index) =>
        StopIndex = index;

    public void AssignBusStopPoints(Vector3 pointerCoordinate, Vector3 stopCoordinate) =>
        _router.AssignBusStopPoints(pointerCoordinate, stopCoordinate);

    private void EndMove(bool isPathCompleted)
    {
        _router.MoveComplited -= EndMove;
        _router.StopArrived -= RemoveRoof;

        StopReleased?.Invoke(this, StopIndex);

        if (isPathCompleted)
            Destroy(gameObject);
        else
            IsActive = true;
    }

    private void RemoveRoof()
    {
        _roof.gameObject.SetActive(false);
    }
}
