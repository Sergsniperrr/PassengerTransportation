using UnityEngine;
using DG.Tweening;

public class ButtonPulsation : Pulsation
{
    private const int EternalCycle = -1;

    private readonly float _scaleMultiplier = 1.1f;

    private Vector3 _initialScale;

    private void Awake()
    {
        _initialScale = transform.localScale;
    }

    protected override Tweener StartAnimation()
    {
        transform.localScale = _initialScale;

        return transform.DOScale(_initialScale * _scaleMultiplier, Duration)
            .SetLoops(EternalCycle, LoopType.Yoyo)
            .SetEase(Ease.Flash);
    }
}
