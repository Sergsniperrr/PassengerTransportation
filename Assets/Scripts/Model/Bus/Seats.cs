using System;
using System.Linq;
using UnityEngine;

public class Seats : MonoBehaviour
{
    [field: SerializeField] public int Count { get; private set; }
    [field: SerializeField] public Vector3 FirstPlaceCoordinate { get; private set; }
    [field: SerializeField] public float SideInterval { get; private set; }
    [field: SerializeField] public float BackInterval { get; private set; }

    private readonly int _failedIndex = -1; 

    private Passenger[] _plases;

    public int FreePlaces => _plases.Where(element => element == null).Count();

    private void Awake()
    {
        _plases = new Passenger[Count];
    }

    public void TakePlace(Passenger passenger, int index)
    {
        int freePlaceIndex = GetFreePlace();

        if (freePlaceIndex == _failedIndex)
            throw new ArgumentOutOfRangeException(nameof(freePlaceIndex));

        _plases[freePlaceIndex] = passenger;
    }

    private int GetFreePlace()
    {
        int result = _failedIndex;

        for (int i = 0; i < Count; i++)
        {
            if (_plases[i] == null)
            {
                result = i;
                break;
            }
        }

        return result;
    }
}
