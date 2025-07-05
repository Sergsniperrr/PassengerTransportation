using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SimpleWindow : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvas;
    [SerializeField] private Button _button;
    [SerializeField] private Background _background;
    [SerializeField] private GameButtons _gameButtons;

    private const float MinTransparent = 0f;
    private const float MaxTransparent = 1f;

    private Tween _fadeTween;

    public event Action<SimpleWindow> Closed;

    private void OnDisable()
    {
        _fadeTween.Kill(false);
    }

    public virtual void Open(float delay = 0f)
    {
        float duration = 0.5f;

        if (delay > 0)
            _button.gameObject.SetActive(false);

        _gameButtons.SetActive(false);
        _background.gameObject.SetActive(true);
        _background.Show(duration);

        Fade(MaxTransparent, duration, (() =>
        {
            StartCoroutine(DelayButtonEnable(delay));

            _button.onClick.AddListener(Close);
        }));
    }

    protected void Close()
    {
        float duration = 0.5f;

        _button.onClick.RemoveListener(Close);
        _background.Hide(duration);

        Closed?.Invoke(this);

        Fade(MinTransparent, duration, (() =>
        {
            gameObject.SetActive(false);
        }));
    }

    private IEnumerator DelayButtonEnable(float delay)
    {
        WaitForSeconds wait = new(delay);

        yield return wait;

        _button.gameObject.SetActive(true);
    }

    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill(false);
        }

        _fadeTween = _canvas.DOFade(endValue, duration);
        _fadeTween.onComplete += onEnd;
    }
}
