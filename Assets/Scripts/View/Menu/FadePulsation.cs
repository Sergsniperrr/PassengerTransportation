using DG.Tweening;
using Scripts.View.Buttons;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FadePulsation : Pulsation
{
    private const float MaxVisibility = 1f;
    private const int EternalCycle = -1;

    private Image _image;
    private Color _initialColor;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _initialColor = _image.color;
    }

    protected override Tweener StartAnimation()
    {
        _image.color = _initialColor;

        return _image.DOFade(MaxVisibility, Duration)
            .SetLoops(EternalCycle, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }
}
