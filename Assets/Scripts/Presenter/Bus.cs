using System;
using UnityEngine;

[RequireComponent(typeof(BusRouter))]
[RequireComponent(typeof(Loader))]
public class Bus : MonoBehaviour
{
    private BusRouter _router;
    private Loader _loader;
    private Roof _roof;

    public event Action<Bus, int> StopReleased;

    public int StopIndex { get; private set; }

    private void Awake()
    {
        _router = GetComponent<BusRouter>();
        _loader = GetComponent<Loader>();
        _roof = GetComponentInChildren<Roof>();

        if (_roof == null)
            throw new NullReferenceException(nameof(_roof));
    }

    public void Run()
    {
        _router.StartMove();

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
    }

    private void RemoveRoof()
    {
        _roof.gameObject.SetActive(false);
    }
}
