using System;
using DG.Tweening;
using Scripts.Model.TrainDoor;
using UnityEngine;

namespace Scripts.Presenters
{
    public class Train : MonoBehaviour
    {
        private const float StartPosition = 25f;
        private const float StationPosition = -25.9f;
        private const float FinalPosition = -100f;
        private const float Duration = 1.3f;

        [SerializeField] private Door _door;

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
            transform.DOMoveX(StationPosition, Duration)
                .SetEase(Ease.OutSine)
                .OnComplete(() => { _door.Open(); });
        }

        public void LeaveStation() =>
            _door.Close();

        private void MoveOutFromStation()
        {
            transform.DOMoveX(FinalPosition, Duration)
                .SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    ChangeVisible(false);

                    LeftStation?.Invoke();
                });
        }

        private void ReturnToStartPosition()
        {
            Vector3 position = transform.position;
            position.x = StartPosition;
            transform.position = position;
        }

        private void FinishMoveToStation() =>
            ArrivedAtStation?.Invoke();

        private void ChangeVisible(bool status)
        {
            foreach (var renderer1 in _renderers)
                renderer1.enabled = status;
        }
    }
}