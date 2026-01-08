using System;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Presenter
{
    public class Train : MonoBehaviour
    {
        [SerializeField] private Door _door;

        private readonly float _startPosition = 25f;
        private readonly float _stationPosition = -25.9f;
        private readonly float _finalPosition = -100f;
        private readonly float _duration = 1.3f;

        private Renderer[] _renderers;

        public event Action ArrivedAtStation;
        public event Action LeftStation;

        private void Awake()
        {
            _renderers = GetComponentsInChildren<Renderer>();
        }

        private void OnEnable()
        {
            _door.Opened += FinishMoveToStation;
            _door.Closed += MoveOutFromStation;
        }

        private void OnDisable()
        {
            _door.Opened -= FinishMoveToStation;
            _door.Closed -= MoveOutFromStation;
        }

        public void MoveToStation()
        {
            ReturnToStartPosition();
            ChangeVisible(true);
            transform.DOMoveX(_stationPosition, _duration)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    _door.Open();
                });
        }

        public void LeaveStation() =>
            _door.Close();

        public void MoveOutFromStation()
        {
            transform.DOMoveX(_finalPosition, _duration)
                .SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    ChangeVisible(false);

                    LeftStation?.Invoke();
                });
        }

        public void ReturnToStartPosition()
        {
            Vector3 position = transform.position;
            position.x = _startPosition;
            transform.position = position;
        }

        private void FinishMoveToStation() =>
            ArrivedAtStation?.Invoke();

        private void ChangeVisible(bool status)
        {
            foreach (Renderer renderer in _renderers)
                renderer.enabled = status;
        }
    }
}
