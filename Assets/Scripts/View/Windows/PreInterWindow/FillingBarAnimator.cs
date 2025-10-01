using UnityEngine;
using DG.Tweening;

public class FillingBarAnimator : MonoBehaviour
{
    private readonly float _maxPositionX = 5.6f;
    private readonly float _maxScaleX = 1.46f;

    private RectTransform _rectTransform;
    private Vector3 _initialScale;
    private Vector2 _initialAnchoredPosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _initialScale = _rectTransform.localScale;
        _initialAnchoredPosition = _rectTransform.anchoredPosition;
    }

    public void Fill(float duration)
    {
        _rectTransform.localScale = _initialScale;
        _rectTransform.anchoredPosition = _initialAnchoredPosition;

        _rectTransform.DOAnchorPosX(_maxPositionX, duration).SetEase(Ease.Linear);
        _rectTransform.DOScaleX(_maxScaleX, duration).SetEase(Ease.Linear);
    }
}
