using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Scripts.View.Menu;

public class Window : MonoBehaviour
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Background _background;
    [SerializeField] private AudioSource _windowsAudio;
    [SerializeField] private GameButtons _buttons;
    [SerializeField] private float _maxScaleMultiplier = 1.1f;

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

        _buttons.gameObject.SetActive(false);
        _background.Show();

        _windowsAudio.Play();

        sequence.Append(transform.DOScale(_maxScale, _scaleDuration))
            .Append(transform.DOScale(_initialScale, _scaleDuration));
    }

    private void Close()
    {
        Sequence sequence = DOTween.Sequence();

        _background.Hide();
        _windowsAudio.Play();

        sequence.Append(transform.DOScale(_maxScale, _outsideDuration))
            .Append(transform.DOScale(Vector3.zero, _scaleDuration))
            .OnComplete(() =>
            {
                Closed?.Invoke();

                _buttons.gameObject.SetActive(true);
                gameObject.SetActive(false);
            });
    }
}
