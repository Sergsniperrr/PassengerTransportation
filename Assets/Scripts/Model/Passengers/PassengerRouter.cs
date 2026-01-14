using System.Collections.Generic;
using Scripts.Model.BusStops;
using Scripts.Presenters;
using UnityEngine;

namespace Scripts.Model.Passengers
{
    [RequireComponent(typeof(PassengerMover))]
    public class PassengerRouter : MonoBehaviour
    {
        private const float FirstPlatformPositionX = -31.45f;
        private const float PlatformIntervalX = 1.6489f;

        private readonly Vector3 _shiftToBusEnter = new (0.48f, 0f, 0.92f);
        private readonly Vector3 _busStopPosition = new (-28.2f, 0.46f, 5.44f);

        private Vector3 _busPlatformPosition = new (0f, 0.46f, 5.44f);
        private PassengerMover _mover;
        private int _queueSize;

        private void Awake()
        {
            _mover = GetComponent<PassengerMover>();
        }

        public void InitializeQueueSize(int size, ISenderOfGettingOnBus sender)
        {
            _mover.InitializeData(sender);
            _queueSize = size;
            _mover.SetRout(CalculateQueuePositions());
        }

        public void IncrementCurrentIndex() =>
            _mover.IncrementCurrentIndex();

        public void SpeedUp() =>
            _mover.SpeedUp();

        public void ResetSpeed() =>
            _mover.ResetSpeed();

        public void SetPlaceIndex(int index) =>
            _mover.SetPlaceIndex(index);

        public void GetOnBus(Bus bus)
        {
            _mover.SetRout(CalculateRoteToBus(bus), false);
        }

        private Vector3[] CalculateQueuePositions()
        {
            const int RotaryIndex = 10;
            const float StepSize = 0.5f;

            Vector3 position = Vector3.zero;
            List<Vector3> positions = new ();

            for (int i = 0; i < _queueSize; i++)
            {
                position.z = Mathf.Min(i, RotaryIndex - 1) * StepSize;
                position.x = Mathf.Max(0, i - RotaryIndex + 1) * StepSize;
                position.x *= -1;

                positions.Add(transform.position + position);
            }

            return positions.ToArray();
        }

        private Vector3[] CalculateRoteToBus(Bus bus)
        {
            List<Vector3> positions = new ();
            _busPlatformPosition.x = PlatformIntervalX * bus.StopIndex + FirstPlatformPositionX;

            positions.Add(_busStopPosition);
            positions.Add(_busPlatformPosition);
            positions.Add(_busPlatformPosition + _shiftToBusEnter);

            return positions.ToArray();
        }
    }
}