using DG.Tweening;
using UnityEngine;

namespace Scripts.View.Buttons
{
    public class ButtonPulsation : Pulsation
    {
        private const int EternalCycle = -1;
        private const float ScaleMultiplier = 1.1f;

        private Vector3 _initialScale;

        private void Awake()
        {
            _initialScale = transform.localScale;
        }

        protected override Tweener StartAnimation()
        {
            transform.localScale = _initialScale;

            return transform.DOScale(_initialScale * ScaleMultiplier, Duration)
                .SetLoops(EternalCycle, LoopType.Yoyo)
                .SetEase(Ease.Flash);
        }
    }
}