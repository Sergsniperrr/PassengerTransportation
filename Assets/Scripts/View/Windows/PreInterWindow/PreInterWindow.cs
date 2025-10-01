using UnityEngine;
using DG.Tweening;
using YG;

[RequireComponent(typeof(CanvasGroup))]
public class PreInterWindow : MonoBehaviour
{
    [SerializeField] private int _waitingTime;
    [SerializeField] private PreInterCounter _counter;
    [SerializeField] private FillingBarAnimator _bar;

    private const float MaxAlfa = 0.8f;

    private readonly float _scaleDuration = 0.3f;

    private CanvasGroup _canvasGroup;
    private Vector3 _initialScale;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _initialScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    public void StartTimer()
    {
        transform.DOScale(_initialScale, _scaleDuration);

        _counter.StartCounting(_waitingTime);
        _bar.Fill(_waitingTime);

        _counter.CountingCompleted += ViewAd;
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
