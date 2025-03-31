using System;
using UnityEngine;

public class BusMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private readonly float _minDistance = 0.3f;
    private readonly float _derectionForward = 1f;
    private readonly float _derectionBackward = -1f;

    private Vector3 _velocity = Vector3.forward;
    private Vector3 _target = Vector3.zero;
    private Vector3 _direction;
    private float _speedDirection = 1f;
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
                MoveToTarget();

            transform.Translate(_velocity * _speedDirection);
        }
    }

    public void Stop() =>
        _canMove = false;

    public void Run()
    {
        _canMove = true;
        _speedDirection = _derectionForward;
    }

    public void GoToPoint(Vector3 point)
    {
        point.y = transform.position.y;
        _target = point;
        _canMove = true;
        _speedDirection = _derectionForward;

        transform.LookAt(_target);
    }

    public void ChangeDirection(Vector3 target)
    {
        target.y = transform.position.y;
        transform.LookAt(target);
        _speedDirection = _derectionForward;
        _canMove = true;
    }

    public void GoBackwardsToPoint(Vector3 point)
    {
        point.y = transform.position.y;
        _target = point;
        _canMove = true;
        _speedDirection = _derectionBackward;
    }

    private void MoveToTarget()
    {
        _direction = _target - transform.position;
        _sqrDistance = _direction.sqrMagnitude;

        if (_sqrDistance < _minDistance)
        {
            transform.position = _target;
            _direction = Vector3.zero;
            _target = Vector3.zero;
            _canMove = false;

            ArrivedAtPoint?.Invoke();
        }
    }
}
