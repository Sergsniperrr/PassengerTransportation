using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class ElevatorView : MonoBehaviour
{
    private List<Transform> _elevatorParts = new();
    private float _duration = 0.2f;
    private float _increaseSize = 1.05f;

    private void Awake()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        Canvas counterCanvas = GetComponentInChildren<Canvas>();

        _elevatorParts.Add(counterCanvas.transform);

        foreach (SpriteRenderer renderer in renderers)
            _elevatorParts.Add(renderer.transform);
    }

    private void OnDisable()
    {
        foreach (Transform part in _elevatorParts)
            part.DOKill();
    }

    public void Hide()
    {
        foreach (Transform part in _elevatorParts)
        {
            part.DOScale(part.localScale * _increaseSize, _duration).SetEase(Ease.OutSine).OnComplete(() => 
            {
                part.DOScale(0, _duration).SetEase(Ease.InSine);
            });

        }
    }
}
