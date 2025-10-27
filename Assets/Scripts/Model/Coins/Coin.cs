using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Coin : SpawnableObject<Coin>
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

    public float CalculateDuration() =>
        Mathf.Abs(_targetPosition.x - transform.position.x) * _multiplier + _shift;

    public void Show(Vector3 targetPosition, bool canUpMove = true, bool isDisableOnFinish = true)
    {
        _isDisableOnFinish = isDisableOnFinish;
        _targetPosition = targetPosition;

        if (canUpMove)
        {
            transform.localPosition = _initialPosition;
            transform.localRotation = Quaternion.identity;
            _newPositionY = _initialPosition.y + _distance;
            transform.DOLocalMoveY(_newPositionY, _beginMoveDuration).OnComplete(MoveToTarget);
        }
        else
        {
            MoveToTarget();
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
        Die(this);

        if (_isDisableOnFinish)
            gameObject.SetActive(false);
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
