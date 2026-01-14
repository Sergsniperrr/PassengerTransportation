using System.Collections;
using DG.Tweening;
using Scripts.Model.Other;
using UnityEngine;

namespace Scripts.Model.Coins
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Coin : SpawnableObject<Coin>
    {
        private const float MaxAlfa = 1f;
        private const float Distance = 1.5f;
        private const float BeginMoveDuration = 0.1f;
        private const float Multiplier = 0.03f;
        private const float Shift = 0.3f;
        private const float FadeDuration = 0.1f;

        private SpriteRenderer _renderer;
        private Vector3 _initialPosition;
        private Vector3 _targetPosition;
        private Color _minAlfaColor;
        private float _newPositionY;
        private bool _isDisableOnFinish;

        public float MoveDuration { get; private set; } = 0.7f;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _initialPosition = transform.position;
            _minAlfaColor = _renderer.color;
            _minAlfaColor.a = 0f;
        }

        public void Show(Vector3 targetPosition, bool canUpMove = true, bool isDisableOnFinish = true)
        {
            _isDisableOnFinish = isDisableOnFinish;
            _targetPosition = targetPosition;

            if (canUpMove)
            {
                transform.localPosition = _initialPosition;
                transform.localRotation = Quaternion.identity;
                _newPositionY = _initialPosition.y + Distance;
                transform.DOLocalMoveY(_newPositionY, BeginMoveDuration).OnComplete(MoveToTarget);
            }
            else
            {
                MoveToTarget();
                StartCoroutine(FadeFast());
            }
        }

        private float CalculateDuration() =>
            Mathf.Abs(_targetPosition.x - transform.position.x) * Multiplier + Shift;

        private void MoveToTarget()
        {
            MoveDuration = CalculateDuration();

            transform.DOMove(_targetPosition, MoveDuration)
                .SetEase(Ease.InSine)
                .OnComplete(Disable);
        }

        private void Disable()
        {
            Die(this);

            if (_isDisableOnFinish)
            {
                gameObject.SetActive(false);
            }
        }

        private IEnumerator FadeFast()
        {
            WaitForSeconds wait = new(FadeDuration);
            Color maxAlfaColor = _minAlfaColor;
            maxAlfaColor.a = MaxAlfa;

            _renderer.color = _minAlfaColor;

            yield return wait;

            _renderer.color = maxAlfaColor;
        }
    }
}