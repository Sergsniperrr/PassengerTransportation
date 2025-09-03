using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneySpendView : SpawnableObject<MoneySpendView>
{
    private readonly float _moveDistance = 150f;
    private readonly float _moveDuration = 1f;
    private readonly float _fadeInDuration = 0.2f;
    private readonly float _fadeOutDuration = 0.4f;
    private readonly float _fadeOutDelay = 0.4f;

    private TextMeshProUGUI _text;
    private Vector3 _initialPosition;
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
        //transform.DOKill();
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
        transform.DOLocalMoveY(transform.localPosition.y + _moveDistance, _moveDuration).SetEase(Ease.OutSine);
    }

    private void FadeIn()
    {
        _text.DOColor(_maxColor, _fadeInDuration).OnComplete(FadeOut);
    }

    private void FadeOut()
    {
        _text.DOColor(_minColor, _fadeOutDuration)
            .SetDelay(_fadeOutDelay)
            .OnComplete(Disable);
    }

    private void Disable() =>
        Die(this);
}
