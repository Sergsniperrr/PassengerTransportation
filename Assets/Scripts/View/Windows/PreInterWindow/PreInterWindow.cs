using UnityEngine;
using DG.Tweening;
using YG;

[RequireComponent(typeof(CanvasGroup))]
public class PreInterWindow : MonoBehaviour
{
    [SerializeField] private int _waitingTime;
    [SerializeField] private PreInterCounter _counter;
    [SerializeField] private FillingBarAnimator _bar;

    private readonly float _scaleDuration = 0.3f;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    public void Close()
    {
        transform.DOScale(Vector3.zero, _scaleDuration)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }

    private void ViewAd()
    {
        _counter.CountingCompleted -= ViewAd;

        YG2.InterstitialAdvShow();

        Close();
    }
}
