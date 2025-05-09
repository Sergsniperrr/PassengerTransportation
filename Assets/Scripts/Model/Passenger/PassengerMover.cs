using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PassengerAnimator))]
public class PassengerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private readonly int _rotaryIndex = 10;
    private readonly float _stepSize = 0.5f;
    private readonly float _maxPositionZForRotation = 4.5f;
    private readonly float _randomDifferenceAngle = 40f;
    private readonly float _speedMultiplier = 2f;
    private readonly Queue<Vector3> _targets = new();

    private Vector3 _zeroPosition;
    private Vector3[] _coordinates;
    private Queue<Vector3> _positionsOfQueue;
    private PassengerAnimator _animator;
    private float _initialSpeed;
    private int _indexOfQueue;
    private int _maxIndex = 24;

    public event Action MoveCompleted;

    public Vector3 Target { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<PassengerAnimator>();
        _initialSpeed = _speed;
        _zeroPosition = transform.position;
    }

    private void OnEnable()
    {
        _indexOfQueue = _maxIndex;
    }

    private void Update()
    {
        Move();
    }

    public void SetPlaceIndex(int index)
    {
        if (index < 0)
            throw new ArgumentOutOfRangeException(nameof(index));

        _indexOfQueue = index;
    }

    public void InitialPositionsOfQueue(Queue<Vector3> positions) =>
        _positionsOfQueue = positions;

    public void MoveTo(Vector3 target) =>
        _targets.Enqueue(target);

    public void SpeedUp() =>
        _speed *= _speedMultiplier;

    public void ResetSpeed() =>
        _speed = _initialSpeed;

    public void MoveToNextPlaceInQueue()
    {
        if (_positionsOfQueue.Count > 0)
            MoveTo(_positionsOfQueue.Dequeue());
    }

    public void SkipPositionsOfQueue(int countPositions)
    {
        for (int i = 0; i < countPositions; i++)
            _positionsOfQueue.Dequeue();
    }

    private void Move()
    {
        if (Target == Vector3.zero)
        {
            if (_targets.Count > 0)
            {
                Target = _targets.Dequeue();
                _animator.Move();
            }
            else
            {
                return;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, Target, _speed * Time.deltaTime);

        if (transform.position == Target)
        {
            if (transform.position.z == _maxPositionZForRotation)
            {
                Vector3 rotationOnRight = new(0f, -90f, 0f);
                transform.rotation = Quaternion.Euler(rotationOnRight);
            }

            if (_targets.Count > 0)
            {
                Target = _targets.Dequeue();
                transform.LookAt(Target);
                return;
            }

            _animator.Stop();
            Target = Vector3.zero;

            MoveCompleted?.Invoke();

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
