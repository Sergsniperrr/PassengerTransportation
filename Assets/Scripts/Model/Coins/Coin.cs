using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Coin : MonoBehaviour
{
    private const float MaxAlfa = 1f;

    private readonly float _distance = 1.5f;
    private readonly float _beginMoveDuration = 0.1f;
    private readonly float _multiplier = 0.03f;
    private readonly float _shift = 0.3f;
    private readonly float _fadeDuration = 0.1f;

    private SpriteRenderer _renderer;
    private Vector3 _initialPosition;
    private Vector3 _targetPosition;
    private Color _minAlfaColor;
    private float newPositionY;

    public event Action<Coin> MoveComplete;

    public float MoveDuration { get; private set; } = 0.7f;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _minAlfaColor = _renderer.color;
        _minAlfaColor.a = 0f;
    }

    public void InitializePosition(Vector3 position)
    {
        _initialPosition = position;
    }

    public float CalculateDuration() =>
        Mathf.Abs(_targetPosition.x - transform.position.x) * _multiplier + _shift;

    public void Show(Vector3 targetPosition, bool canUpMove = true)
    {
        _targetPosition = targetPosition;
        newPositionY = _initialPosition.y + _distance;
        transform.localPosition = _initialPosition;
        transform.localRotation = Quaternion.identity;

        if (canUpMove)
        {
            transform.DOLocalMoveY(newPositionY, _beginMoveDuration).OnComplete(MoveToTarget);
        }
        else
        {
            MoveToTarget();
            //FadeIn();
            StartCoroutine(FadeFast());
        }
    }

    private void MoveToTarget()
    {
        MoveDuration = CalculateDuration();

        transform.DOMove(_targetPosition, MoveDuration)
            .SetEase(Ease.InSine)
            .OnComplete(Disable);
    }

    private void Disable()
    {
        MoveComplete?.Invoke(this);

        transform.DOKill();
        gameObject.SetActive(false);
    }

    private void FadeIn()
    {
        _renderer.color = _minAlfaColor;
        _renderer.DOFade(MaxAlfa, _fadeDuration);
    }

    private IEnumerator FadeFast()
    {
        WaitForSeconds wait = new(_fadeDuration);
        Color maxAlfaColor = _minAlfaColor;
        maxAlfaColor.a = MaxAlfa;

        _renderer.color = _minAlfaColor;

        yield return wait;

        _renderer.color = maxAlfaColor;
    }
}
