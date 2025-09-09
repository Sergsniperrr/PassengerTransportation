using System;
using TMPro;
using DG.Tweening;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CoinsCounterView : MonoBehaviour
{
    [SerializeField] private Effects _effects;

    private TextMeshProUGUI _text;
    private Tween _tween;
    private Coroutine _coroutine;
    private float _scaleDuration = 0.2f;

    public event Action ChangeCompleted;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void SetValue(int value) =>
        _text.text = $"{value}";

    public void ChangeValue(int currentValue, int newValue, float duration = 0)
    {
        if (newValue < 0)
            throw new ArgumentOutOfRangeException(nameof(newValue));

        if (duration == 0)
        {
            _text.text = $"{newValue}";
            PulsateText();
        }
        else
        {
            ChangeValueSmooth(currentValue, newValue, duration);
        }
    }

    private void PulsateText()
    {
        if (_tween != null)
            return;

        _tween = _text.transform.DOPunchScale(_text.transform.localScale, _scaleDuration)
            .OnComplete(() =>
            {
                _text.transform.DOKill();
                _tween = null;
            });
    }

    private void ChangeValueSmooth(int currentValue, int newValue, float duration)
    {
        int delta = Mathf.Abs(newValue - currentValue);

        if (delta == 0)
            return;

        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(PlayCountingCoinsSound());

        DOTween.To(
            () => currentValue,
            x => currentValue = x,
            newValue,
            duration
        )
        .SetEase(Ease.Linear)
        .OnUpdate(() =>
        {
            _text.text = currentValue.ToString();
        })
        .OnComplete(() => 
        {
            StopCoroutine(_coroutine);
            ChangeCompleted?.Invoke();
        });
    }

    private IEnumerator PlayCountingCoinsSound()
    {
        WaitForSeconds wait = new(0.1f);

        while (enabled)
        {
            _effects.PlayCoinsAudio();

            yield return wait;
        }
    }
}
