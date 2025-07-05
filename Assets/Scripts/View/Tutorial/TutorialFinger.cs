using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class TutorialFinger : MonoBehaviour
{
    [SerializeField] private Vector3[] _points;
    [SerializeField] private Level _level;

    private const int EternalCycle = -1;
    private const string LevelPrefName = "CurrentLevel";

    private Bus _bus;
    private Queue<Bus> _buses;
    private Tweener _tweener;
    private Queue<Vector3> _pointsQueue;
    private Vector3 _hiddenPosition;
    private Vector3 _lastPosition;
    private float _shift = 1f;
    private float _moveDuration = 0.25f;

    private void Awake()
    {
        _pointsQueue = new Queue<Vector3>(_points);

        int currentLevel = PlayerPrefs.GetInt(LevelPrefName, 0);

        if (currentLevel > 0)
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _level.GameActivated += Activate;
    }

    private void OnDisable()
    {
        if (_bus != null)
            _bus.LeftParkingLot -= PointNextPosition;

        _tweener.Kill();
    }

    private void Activate(Queue<Bus> buses)
    {
        _level.GameActivated -= Activate;

        _buses = buses ?? throw new ArgumentNullException(nameof(buses));

        PointNextPosition();
    }

    private void PointNextPosition()
    {
        if (_pointsQueue.Count > 0)
        {
            if (_bus != null)
                _bus.LeftParkingLot -= PointNextPosition;

            _lastPosition = _pointsQueue.Dequeue();
            _bus = _buses.Dequeue();
            transform.localPosition = _lastPosition;
            Move();

            _bus.StartedMove += Hide;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void Move()
    {
        float positionY = transform.localPosition.y;

        _tweener = transform.DOLocalMoveY(positionY + _shift, _moveDuration).SetLoops(EternalCycle, LoopType.Yoyo)
            .From()
            .SetEase(Ease.Flash);
    }

    private void Hide()
    {
        _bus.StartedMove -= Hide;

        _tweener.Kill();
        transform.position = _hiddenPosition;

        _bus.LeftParkingLot += PointNextPosition;
    }
}
