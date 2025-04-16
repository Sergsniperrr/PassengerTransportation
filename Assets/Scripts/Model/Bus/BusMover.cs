using System;
using UnityEngine;

public class BusMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private readonly float _minDistance = 0.9f;
    private readonly float _directionForward = 1f;
    private readonly float _directionBackward = -1f;

    private Vector3 _velocity = Vector3.forward;
    private Vector3 _target = Vector3.zero;
    private Vector3 _vectorDistance;
    private float _direction = 1f;
    private float _sqrDistance;
    private bool _canMove = false;

    public event Action ArrivedAtPoint;

    private void Awake()
    {
        _velocity.z = _speed;
    }

    private void Update()
    {
        if (_canMove)
        {
            if (_target != Vector3.zero)
                HandleMoving();

            transform.Translate(_direction * Time.deltaTime * _velocity);
        }
    }

    public void Stop() =>
        _canMove = false;

    public void Run()
    {
        _canMove = true;
        _direction = _directionForward;
    }

    public void GoToPoint(Vector3 point, bool canRotate = true)
    {
        point.y = transform.position.y;
        _target = point;
        _canMove = true;
        _direction = _directionForward;

        if (canRotate)
            transform.LookAt(_target);
    }

    public void ChangeDirection(Vector3 target)
    {
        target.y = transform.position.y;
        transform.LookAt(target);
        _direction = _directionForward;
        _canMove = true;
    }

    public void GoBackwardsToPoint(Vector3 point)
    {
        point.y = transform.position.y;
        _target = point;
        _canMove = true;
        _direction = _directionBackward;
    }

    private void HandleMoving()
    {
        _vectorDistance = _target - transform.position;
        _sqrDistance = _vectorDistance.sqrMagnitude;

        if (_sqrDistance < _minDistance)
        {
            transform.position = _target;
            _vectorDistance = Vector3.zero;
            _target = Vector3.zero;
            _canMove = false;

            ArrivedAtPoint?.Invoke();
        }
    }
}
