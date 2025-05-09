using System;
using System.Linq;
using UnityEngine;

public class Loader : MonoBehaviour
{
    [field: SerializeField] public int Count { get; private set; }
    [field: SerializeField] public Vector3 FirstPlaceCoordinate { get; private set; }
    [field: SerializeField] public float SideInterval { get; private set; }
    [field: SerializeField] public float BackInterval { get; private set; }

    private readonly int _failedIndex = -1;

    private Vector3[] _coordinates;
    private bool[] _reservations;
    private Passenger[] _places;

    public event Action FillingCompleted;

    public int EmptySeatCount => _reservations.Where(element => element == true).Count();
    public bool IsEmptySeat => EmptySeatCount > 0;

    private void Awake()
    {
        _reservations = new bool[Count];
        _places = new Passenger[Count];
        _coordinates = CalculatePlacesCoordinates();

        for (int i = 0; i < Count; i++)
            _reservations[i] = true;
    }

    public int ReserveFreePlace()
    {
        int freePlaceIndex = GetFreePlace();

        if (freePlaceIndex != _failedIndex)
            _reservations[freePlaceIndex] = false;

        return freePlaceIndex;
    }

    public void TakePassenger(Passenger passenger)
    {
        if (passenger == null)
            throw new ArgumentNullException(nameof(passenger));

        passenger.transform.SetParent(transform);
        passenger.transform.localPosition = _coordinates[passenger.BusPlaceIndex];
        passenger.transform.localRotation = Quaternion.Euler(Vector3.zero);

        _places[passenger.BusPlaceIndex] = passenger;

        if (CheckFill())
            FillingCompleted?.Invoke();
    }

    public Passenger GetPassengerByIndex(int index) =>
        _places[index];

    private bool CheckFill()
    {
        foreach (Passenger passenger in _places)
        {
            if (passenger == null)
                return false;
        }

        return true;
    }

    private int GetFreePlace()
    {
        for (int i = 0; i < Count; i++)
        {
            if (_reservations[i])
                return i;
        }

        return _failedIndex;
    }

    private Vector3[] CalculatePlacesCoordinates()
    {
        Vector3[] coordinates = new Vector3[Count];
        Vector3 buffer = Vector3.zero;
        int countInRow = 2;

        for (int i = 0; i < Count; i++)
        {
            buffer.x = i % countInRow * SideInterval;
            buffer.z = i / countInRow * BackInterval;

            coordinates[i] = FirstPlaceCoordinate + buffer;
        }

        return coordinates;
    }
}
