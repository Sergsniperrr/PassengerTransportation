using System;
using UnityEngine;

[RequireComponent(typeof(Bus))]
[RequireComponent(typeof(Collider))]
public class DirectionHandler : MonoBehaviour
{
    private readonly float _busStopPositionZ = 9.96f;
    private readonly float _minPositionX = -31.9f;
    private readonly float _maxPositionX = -19.34f;
    private readonly float _maxPositionZ = 23.47f;
    private readonly float _verticalCentralAxis = -25.59f;
    private readonly float _upDirectionY = 180f;
    private readonly float _rightDirectionY = -90f;
    private readonly float _leftDirectionY = 90f;

    private Collider _collider;
    private IParkingExitHandler _bus;
    private Vector3 _position;

    public event Action BusStopArrived;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _bus = GetComponent<Bus>();
    }

    private void Update()
    {
        HandlePosition();
    }

    private void HandlePosition()
    {
        _position = transform.position;

        if (_position.z < _busStopPositionZ)
        {
            _position.z = _busStopPositionZ;
            transform.position = _position;

            HandleParkingExit();

            BusStopArrived?.Invoke();

            enabled = false;
        }
        else if (_position.x < _minPositionX)
        {
            SetUpDirection(_minPositionX);
        }
        else if (_position.x > _maxPositionX)
        {
            SetUpDirection(_maxPositionX);
        }
        else if (_position.z > _maxPositionZ)
        {
            SetHorizontalDirection();
        }
    }

    private void SetHorizontalDirection()
    {
        _position.z = _maxPositionZ;

        if (_position.x < _verticalCentralAxis)
            transform.SetPositionAndRotation(_position, Quaternion.Euler(0f, _rightDirectionY, 0f));
        else
            transform.SetPositionAndRotation(_position, Quaternion.Euler(0f, _leftDirectionY, 0f));

        HandleParkingExit();
    }

    private void SetUpDirection(float positionX)
    {
        _position.x = positionX;
        transform.SetPositionAndRotation(_position, Quaternion.Euler(0f, _upDirectionY, 0f));

        HandleParkingExit();
    }

    private void HandleParkingExit()
    {
        if (_collider.enabled == true)
            _collider.enabled = false;

        _bus.HandleParkingExit();
    }
}
