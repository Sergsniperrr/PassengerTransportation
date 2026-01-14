using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Scripts.Model.Levels;
using Scripts.Presenters;
using UnityEngine;

namespace Scripts.View.Tutorial
{
    public class TutorialFinger : MonoBehaviour
    {
        private const int EternalCycle = -1;
        private const float Shift = 1f;
        private const float MoveDuration = 0.25f;

        private readonly WaitForSeconds _waitForGameActivity = new (1f);

        [SerializeField] private Vector3[] _points;
        [SerializeField] private Level _level;

        private Bus _bus;
        private Queue<Bus> _buses;
        private Tweener _tweener;
        private Queue<Vector3> _pointsQueue;
        private Vector3 _hiddenPosition;
        private Vector3 _lastPosition;

        private void OnDisable()
        {
            if (_bus != null)
                _bus.LeftParkingLot -= PointNextPosition;

            _tweener.Kill();
        }

        public void Enable()
        {
            _pointsQueue = new Queue<Vector3>(_points);

            _level.GameActivated += Activate;
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

            _tweener = transform.DOLocalMoveY(positionY + Shift, MoveDuration)
                .SetLoops(EternalCycle, LoopType.Yoyo)
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
}