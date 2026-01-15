using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Menu
{
    [RequireComponent(typeof(Image))]
    public class Background : MonoBehaviour
    {
        private const float DefaultFadeDuration = 0.5f;

        [SerializeField] private float _maxDarkness = 0.6f;

        private Image _image;
        private Tween _fadeTween;
        private BoxCollider _collider;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _collider = GetComponentInChildren<BoxCollider>();

            DisableCollider();
        }

        public void Show(float duration = DefaultFadeDuration)
        {
            Fade(_maxDarkness, duration, DisableCollider);
        }

        public void Hide(float duration = DefaultFadeDuration)
        {
            Fade(0, duration, DisableCollider);
        }

        private void DisableCollider()
        {
            if (_collider != null)
                _collider.enabled = false;
        }

        private void Fade(float endValue, float duration, TweenCallback onEnd)
        {
            if (_fadeTween != null)
                _fadeTween.Kill();

            _fadeTween = _image.DOFade(endValue, duration);
            _fadeTween.onComplete += onEnd;
        }
    }
}