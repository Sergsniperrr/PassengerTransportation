using DG.Tweening;
using UnityEngine;

namespace Scripts.View.Windows.LevelCompleteWindows
{
    public class GlowPulsation : MonoBehaviour
    {
        private const float Duration = 0.3f;

        private Vector3 _pulsationScale = new (0.2f, 0.2f, 0f);
        private Tween _tween;
        private Vector3 _initialScale;

        private void Awake()
        {
            _initialScale = transform.localScale;
            _pulsationScale += _initialScale;
        }

        private void OnEnable()
        {
            transform.localScale = _initialScale;
            Pulsate();
        }

        private void OnDisable()
        {
            _tween.Kill();
        }

        private void Pulsate()
        {
            _tween = transform.DOScale(_pulsationScale, Duration).SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}