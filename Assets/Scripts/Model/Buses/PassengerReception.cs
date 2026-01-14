using System;
using System.Linq;
using Scripts.Presenters;
using UnityEngine;

namespace Scripts.Model.Buses
{
    [RequireComponent(typeof(Bus))]
    public class PassengerReception : MonoBehaviour
    {
        private const int FailedIndex = -1;

        private readonly Vector3 _passengerLocalScale = new (0.76f, 0.94f, 0.89f);

        [field: SerializeField] public int Count { get; private set; }
        [field: SerializeField] public Vector3 FirstPlaceCoordinate { get; private set; }
        [field: SerializeField] public float SideInterval { get; private set; }
        [field: SerializeField] public float BackInterval { get; private set; }

        private Vector3[] _coordinates;
        private bool[] _reservations;
        private int _passengersCounter;
        private ISenderOfFillingCompletion _sender;

        public int EmptySeatCount => _reservations.Count(element => element == true);
        public bool IsEmptySeat => EmptySeatCount > 0;

        private void Awake()
        {
            _sender = GetComponent<Bus>();

            _reservations = new bool[Count];
            _coordinates = CalculatePlacesCoordinates();

            for (int i = 0; i < Count; i++)
                _reservations[i] = true;
        }

        public int ReserveFreePlace()
        {
            int freePlaceIndex = GetFreePlace();

            if (freePlaceIndex != FailedIndex)
                _reservations[freePlaceIndex] = false;

            return freePlaceIndex;
        }

        public void TakePassenger(Passenger passenger)
        {
            if (passenger == null)
                throw new ArgumentNullException(nameof(passenger));

            passenger.transform.SetParent(transform);
            passenger.transform.localPosition = _coordinates[_passengersCounter];
            passenger.transform.localRotation = Quaternion.Euler(Vector3.zero);
            passenger.gameObject.transform.localScale = _passengerLocalScale;

            _passengersCounter++;

            if (_passengersCounter == Count)
            {
                _sender.CompleteFilling();
            }
        }

        private int GetFreePlace()
        {
            for (int i = 0; i < Count; i++)
            {
                if (_reservations[i])
                    return i;
            }

            return FailedIndex;
        }

        private Vector3[] CalculatePlacesCoordinates()
        {
            const int CountInRow = 2;

            Vector3[] coordinates = new Vector3[Count];
            Vector3 buffer = Vector3.zero;

            for (int i = 0; i < Count; i++)
            {
                buffer.x = i % CountInRow * SideInterval;
                buffer.z = i / CountInRow * BackInterval;

                coordinates[i] = FirstPlaceCoordinate + buffer;
            }

            return coordinates;
        }
    }
}