using System;
using DG.Tweening;
using Scripts.View.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Windows
{
    public class Window : MonoBehaviour
    {
        private const float OutsideDuration = 0.05f;
        private const float ScaleDuration = 0.2f;

        [SerializeField] private Button _closeButton;
        [SerializeField] private Background _background;
        [SerializeField] private AudioSource _windowsAudio;
        [SerializeField] private GameButtons _buttons;
        [SerializeField] private float _maxScaleMultiplier = 1.1f;

        private Vector3 _initialScale;
        private Vector3 _maxScale;

        public event Action Closed;

        private void Awake()
        {
            _initialScale = transform.localScale;
            float maxScale = _initialScale.x * _maxScaleMultiplier;
            _maxScale = new Vector3(maxScale, maxScale, maxScale);
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

            sequence.Append(transform.DOScale(_maxScale, ScaleDuration))
                .Append(transform.DOScale(_initialScale, ScaleDuration));
        }

        private void Close()
        {
            Sequence sequence = DOTween.Sequence();

            _background.Hide();
            _windowsAudio.Play();

            sequence.Append(transform.DOScale(_maxScale, OutsideDuration))
                .Append(transform.DOScale(Vector3.zero, ScaleDuration))
                .OnComplete(() =>
                {
                    Closed?.Invoke();

                    _buttons.gameObject.SetActive(true);
                    gameObject.SetActive(false);
                });
        }
    }
}