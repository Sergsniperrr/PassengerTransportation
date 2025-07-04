using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Window : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Image _background;
    [SerializeField] private AudioSource _windowsAudio;
    [SerializeField] private float _maxScaleMultiplier = 1.1f;

    private Color _darkBackground = new(0, 0, 0, 0.6f);
    private Vector3 _initialScale;
    private Vector3 _maxScale;
    private float _outsideDuration = 0.05f;
    private float _scaleDuration = 0.2f;

    public event Action Closed;

    private void Awake()
    {
        _initialScale = transform.localScale;
        float maxScale = _initialScale.x * _maxScaleMultiplier;
        _maxScale = new(maxScale, maxScale, maxScale);
        transform.localScale = Vector3.zero;
        _background.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(Close);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(Close);
    }

    public void Open()
    {
        Sequence sequence = DOTween.Sequence();

        _background.DOColor(_darkBackground, _outsideDuration);
        _background.raycastTarget = true;

        _windowsAudio.Play();

        sequence.Append(transform.DOScale(_maxScale, _scaleDuration))
            .Append(transform.DOScale(_initialScale, _scaleDuration));
    }

    private void Close()
    {
        Sequence sequence = DOTween.Sequence();

        _background.DOColor(Color.clear, _outsideDuration);

        _windowsAudio.Play();

        sequence.Append(transform.DOScale(_maxScale, _outsideDuration))
            .Append(transform.DOScale(Vector3.zero, _scaleDuration))
            .OnComplete(() =>
            {
                Closed?.Invoke();

                _background.raycastTarget = false;
                gameObject.SetActive(false);
            });
    }
}
