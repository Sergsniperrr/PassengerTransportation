using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Background : MonoBehaviour
{
    [SerializeField] private float _maxDarkness = 0.6f;

    private Image _image;
    private Tween _fadeTween;
    private BoxCollider _collider;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _collider = GetComponentInChildren<BoxCollider>();

        if (_collider != null)
            _collider.enabled = false;
    }

    public void Show(float duration = 0.5f)
    {
        Fade(_maxDarkness, duration, (() =>
        {
            if (_collider != null)
                _collider.enabled = true;
        }));
    }

    public void Hide(float duration = 0.5f)
    {
        Fade(0, duration, (() =>
        {
            if (_collider != null)
                _collider.enabled = false;
        }));
    }

    private void Fade(float endValue, float duration, TweenCallback onEnd)
    {
        if (_fadeTween != null)
            _fadeTween.Kill(false);

        _fadeTween = _image.DOFade(endValue, duration);
        _fadeTween.onComplete += onEnd;
    }
}
