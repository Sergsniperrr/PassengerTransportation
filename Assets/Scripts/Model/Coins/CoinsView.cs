using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CoinsView : MonoBehaviour
{
    private const float MaxAlfa = 1f;
    private const float MinAlfa = 0f;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show() =>
        _canvasGroup.alpha = MaxAlfa;

    public void Hide() =>
        _canvasGroup.alpha = MinAlfa;
}
