using System;
using TMPro;
using DG.Tweening;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class CoinsCounterView : MonoBehaviour
{
    [SerializeField] private Effects _effects;
    //[SerializeField] private TemporaryCounter _temporaryCounter;

    private readonly float _durationPerOne = 0.06f;

    private TextMeshProUGUI _text;
    private Tween _tween;
    private Coroutine _coroutine;
    private float _scaleDuration = 0.2f;

    public event Action ValueChanged;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void SetValue(int value) =>
        _text.text = $"{value}";

    public void ChangeValue(int currentValue, int newValue, bool isFastChange)
    {
        if (newValue < 0)
            throw new ArgumentOutOfRangeException(nameof(newValue));


        if (isFastChange)
        {
            _text.text = $"{newValue}";
            PulsateText();
        }
        else
        {
            ChangeValueSmooth(currentValue, newValue);
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

    private void ChangeValueSmooth(int currentValue, int newValue)
    {
        int delta = Mathf.Abs(newValue - currentValue);

        if (delta == 0)
            return;

        _coroutine = StartCoroutine(PlayCountingCoinsSound());

        DOTween.To(
            () => currentValue,
            x => currentValue = x,
            newValue,
            _durationPerOne * delta
        )
        .SetEase(Ease.Linear)
        .OnUpdate(() =>
        {
            _text.text = currentValue.ToString();
        })
        .OnComplete(() => 
        {
            StopCoroutine(_coroutine);
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
