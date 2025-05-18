using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PassengerAnimator))]
public class PassengerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private const int FailedIndex = -1;

    private readonly float _randomDifferenceAngle = 40f;
    private readonly float _speedMultiplier = 2f;
    private readonly Queue<Vector3> _targets = new();

    private ISenderOfGettingOnBus _sender;
    private Vector3[] _positions;
    private Vector3 _velocity = Vector3.forward;
    private PassengerAnimator _animator;
    private float _initialSpeed;
    private bool _isInQueue = true;

    public int CurrentPosition { get; private set; }

    public Vector3 Target { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<PassengerAnimator>();
        _initialSpeed = _speed;
        _velocity.z = _speed;
    }

    private void Update()
    {
        Move();
    }

    public void InitializeData(ISenderOfGettingOnBus sender) =>
        _sender = sender;

    public void SetRout(Vector3[] positions, bool isInQueue = true)
    {
        _positions = positions;
        SetPlaceIndex(0);
        _isInQueue = isInQueue;
    }

    public void IncrementCurrentIndex()
    {
        if (CurrentPosition < _positions.Length - 1)
        {
            SetPlaceIndex(CurrentPosition + 1);
        }
    }

    public void SetPlaceIndex(int index)
    {
        CurrentPosition = index;
        transform.LookAt(_positions[CurrentPosition]);
        _animator.Move();
    }

    public void SpeedUp() =>
        _speed *= _speedMultiplier;

    public void ResetSpeed() =>
        _speed = _initialSpeed;

    private void Move()
    {
        if (CurrentPosition == FailedIndex)
            return;

        if (_positions.Length > 0 && transform.position != _positions[CurrentPosition])
        {
            transform.position = Vector3.MoveTowards(transform.position, _positions[CurrentPosition], _speed * Time.deltaTime);
        }
        else if (_isInQueue == false && CurrentPosition < _positions.Length - 1)
        {
            SetPlaceIndex(CurrentPosition + 1);
        }
        else
        {
            if (_isInQueue == false && CurrentPosition == _positions.Length - 1)
            {
                _sender.SendGettingOnBusAction();
                CurrentPosition = FailedIndex;
            }

            _animator.Stop();
        }
    }

    //private void RotateRandom()
    //{
    //    float angle = UnityEngine.Random.Range(0f, _randomDifferenceAngle);
    //    Vector3 rotationAngle = transform.rotation.eulerAngles;

    //    rotationAngle.y += angle;

    //    transform.rotation = Quaternion.Euler(rotationAngle);
    //}
}
