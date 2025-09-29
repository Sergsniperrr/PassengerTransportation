using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using YG;
using System.Collections;

public class TutorialFinger : MonoBehaviour
{
    [SerializeField] private Vector3[] _points;
    [SerializeField] private Level _level;

    private const int EternalCycle = -1;

    private Bus _bus;
    private Queue<Bus> _buses;
    private Tweener _tweener;
    private Queue<Vector3> _pointsQueue;
    private Vector3 _hiddenPosition;
    private Vector3 _lastPosition;
    private WaitForSeconds _waitForGameActivity = new(1f);
    private float _shift = 1f;
    private float _moveDuration = 0.25f;

    private void OnEnable()
    {
        if (YG2.saves.Level == 1)
        {
            _pointsQueue = new Queue<Vector3>(_points);

            _level.GameActivated += Activate;

        }
        else
        {
            gameObject.SetActive(false);
        }

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

        PointNextPosition(null);
    }

    private void PointNextPosition(Bus _)
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

        StartCoroutine(DelayBeforeGameActivity());

        _tweener.Kill();
        transform.position = _hiddenPosition;

        _bus.LeftParkingLot += PointNextPosition;
    }

    private IEnumerator DelayBeforeGameActivity()
    {
        _level.ChangeGameActivity(false);

        yield return _waitForGameActivity;

        _level.ChangeGameActivity(true);
    }
}
