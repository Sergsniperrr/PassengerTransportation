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

    protected const float MinTransparent = 0f;
    protected const float MaxTransparent = 1f;

    protected readonly float FadeDuration = 0.5f; 

    private Tween _fadeTween;

    public event Action<SimpleWindow> Closed;

    private void OnDisable()
    {
        _fadeTween.Kill(false);
    }

    public virtual void Open(float delay = 0f)
    {
        if (delay > 0)
            _button.gameObject.SetActive(false);

        _gameButtons.gameObject.SetActive(false);
        _background.gameObject.SetActive(true);
        _background.Show(FadeDuration);

        Fade(MaxTransparent, FadeDuration, (() =>
        {
            if (delay >= 0)
                StartCoroutine(DelayButtonEnable(delay));

            _button.onClick.AddListener(Close);
        }));
    }

    protected void EnableOkButton() =>
        _button.gameObject.SetActive(true);

    protected void DisableOkButton() =>
        _button.gameObject.SetActive(false);

    protected void Close()
    {
        _button.onClick.RemoveListener(Close);
        _background.Hide(FadeDuration);

        Closed?.Invoke(this);

        Fade(MinTransparent, FadeDuration, (() =>
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

        _canvas.DOKill();
        _fadeTween = _canvas.DOFade(endValue, duration);
        _fadeTween.onComplete += onEnd;
    }
}
