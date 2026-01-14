using System;
using System.Collections;
using DG.Tweening;
using Scripts.Sounds;
using TMPro;
using UnityEngine;

namespace Scripts.Model.Coins
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class CoinsCounterView : MonoBehaviour
    {
        private const float ScaleDuration = 0.2f;

        [SerializeField] private Effects _effects;

        private TextMeshProUGUI _text;
        private Tween _tween;
        private Coroutine _coroutine;

        public event Action ChangeCompleted;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        public void SetValue(int value)
        {
            _text.text = value.ToString();
        }

        public void ChangeValue(int currentValue, int newValue, float duration = 0f)
        {
            if (newValue < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(newValue));
            }

            if (duration == 0f)
            {
                _text.text = newValue.ToString();
                PulsateText();
                return;
            }

            ChangeValueSmooth(currentValue, newValue, duration);
        }

        private void PulsateText()
        {
            if (_tween != null)
            {
                return;
            }

            _tween = _text.transform
                .DOPunchScale(_text.transform.localScale, ScaleDuration)
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
            {
                return;
            }

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _coroutine = StartCoroutine(PlayCountingCoinsSound());

            DOTween.To(() => currentValue, x => currentValue = x, newValue, duration)
                .SetEase(Ease.Linear)
                .OnUpdate(() => _text.text = currentValue.ToString())
                .OnComplete(() =>
                {
                    if (_coroutine != null)
                    {
                        StopCoroutine(_coroutine);
                        _coroutine = null;
                    }

                    ChangeCompleted?.Invoke();
                });
        }

        private IEnumerator PlayCountingCoinsSound()
        {
            WaitForSeconds wait = new WaitForSeconds(0.1f);

            while (enabled)
            {
                _effects.PlayCoinsAudio();
                yield return wait;
            }
        }
    }
}