using System;
using DG.Tweening;
using Scripts.Model.Other;
using TMPro;
using UnityEngine;

namespace Scripts.Model.Money
{
    public class MoneySpendView : SpawnableObject<MoneySpendView>
    {
        private const float MoveDistance = 150f;
        private const float MoveDuration = 1f;
        private const float FadeInDuration = 0.2f;
        private const float FadeOutDuration = 0.4f;
        private const float FadeOutDelay = 0.4f;

        private TextMeshProUGUI _text;
        private Color _maxColor;
        private Color _minColor;

        private void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();

            if (_text == null)
                throw new NullReferenceException(nameof(_text));

            _maxColor = _text.faceColor;
            _minColor = _text.faceColor;

            _minColor.a = 0;
        }

        private void OnDisable()
        {
            _text.DOKill();
        }

        public void PlayBuyEffect(Vector3 position, int price)
        {
            _text.text = $"-{price}";
            transform.position = position;

            Move();
            FadeIn();
        }

        private void Move()
        {
            transform.DOLocalMoveY(transform.localPosition.y + MoveDistance, MoveDuration).SetEase(Ease.OutSine);
        }

        private void FadeIn()
        {
            _text.DOColor(_maxColor, FadeInDuration).OnComplete(FadeOut);
        }

        private void FadeOut()
        {
            _text.DOColor(_minColor, FadeOutDuration)
                .SetDelay(FadeOutDelay)
                .OnComplete(Disable);
        }

        private void Disable() =>
            Die(this);
    }
}