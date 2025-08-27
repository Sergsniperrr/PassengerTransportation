using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CoinsView : MonoBehaviour
{
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show() =>
        _canvasGroup.alpha = 1f;

    public void Hide() =>
        _canvasGroup.alpha = 0f;
}
