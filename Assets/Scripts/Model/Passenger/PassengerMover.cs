using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PassengerAnimator))]
public class PassengerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private readonly float _maxPositionZForRotation = 4.5f;
    private readonly Queue<Vector3> _targets = new();

    private PassengerAnimator _animator;
    private Vector3 _target = Vector3.zero;
    private float _randomDifferenceAngle = 40f;
    private bool _isMoveCompleted;

    public event Action ArrivedToBus;

    private void Awake()
    {
        _animator = GetComponent<PassengerAnimator>();
    }

    private void Update()
    {
        Move();
    }

    public void MoveTo(Vector3 target) =>
        _targets.Enqueue(target);

    public void FinishMove(Vector3 target)
    {
        MoveTo(target);
        _isMoveCompleted = true;
    }

    private void Move()
    {
        if (_target == Vector3.zero)
        {
            if (_targets.Count > 0)
            {
                _target = _targets.Dequeue();
                _animator.Move();
            }
            else
            {
                return;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);

        if (transform.position == _target)
        {
            if (transform.position.z == _maxPositionZForRotation)
            {
                Vector3 rotationOnRight = new(0f, -90f, 0f);
                transform.rotation = Quaternion.Euler(rotationOnRight);
            }

            if (_targets.Count > 0)
            {
                _target = _targets.Dequeue();
                transform.LookAt(_target);
                return;
            }

            _animator.Stop();
            _target = Vector3.zero;

            if (_isMoveCompleted)
            {
                ArrivedToBus?.Invoke();
                return;
            }

            RotateRandom();
        }
    }

    private void RotateRandom()
    {
        float angle = UnityEngine.Random.Range(0f, _randomDifferenceAngle);
        Vector3 rotationAngle = transform.rotation.eulerAngles;

        rotationAngle.y += angle;

        transform.rotation = Quaternion.Euler(rotationAngle);
    }
}
