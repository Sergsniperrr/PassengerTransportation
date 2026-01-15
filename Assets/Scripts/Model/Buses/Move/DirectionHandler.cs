using System;
using UnityEngine;

namespace Scripts.Model.Buses.Move
{
    [RequireComponent(typeof(Presenters.Bus))]
    [RequireComponent(typeof(Collider))]
    public class DirectionHandler : MonoBehaviour
    {
        private const float BusStopPositionZ = 9.96f;
        private const float MinPositionX = -31.9f;
        private const float MaxPositionX = -19.34f;
        private const float MaxPositionZ = 23.47f;
        private const float VerticalCentralAxis = -25.59f;
        private const float UpDirectionY = 180f;
        private const float RightDirectionY = -90f;
        private const float LeftDirectionY = 90f;

        private Collider _collider;
        private IParkingExitHandler _bus;
        private Vector3 _position;

        public event Action BusStopArrived;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _bus = GetComponent<Presenters.Bus>();
        }

        private void Update()
        {
            HandlePosition();
        }

        private void HandlePosition()
        {
            _position = transform.position;

            if (_position.z < BusStopPositionZ)
            {
                _position.z = BusStopPositionZ;
                transform.position = _position;

                HandleParkingExit();

                BusStopArrived?.Invoke();

                enabled = false;
            }
            else if (_position.x < MinPositionX)
            {
                SetUpDirection(MinPositionX);
            }
            else if (_position.x > MaxPositionX)
            {
                SetUpDirection(MaxPositionX);
            }
            else if (_position.z > MaxPositionZ)
            {
                SetHorizontalDirection();
            }
        }

        private void SetHorizontalDirection()
        {
            _position.z = MaxPositionZ;
            float direction = _position.x < VerticalCentralAxis ? RightDirectionY : LeftDirectionY;

            transform.SetPositionAndRotation(_position, Quaternion.Euler(0f, direction, 0f));

            HandleParkingExit();
        }

        private void SetUpDirection(float positionX)
        {
            _position.x = positionX;
            transform.SetPositionAndRotation(_position, Quaternion.Euler(0f, UpDirectionY, 0f));

            HandleParkingExit();
        }

        private void HandleParkingExit()
        {
            if (_collider.enabled)
                _collider.enabled = false;

            _bus.HandleParkingExit();
        }
    }
}