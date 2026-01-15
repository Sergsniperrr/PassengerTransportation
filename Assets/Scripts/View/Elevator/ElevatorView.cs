using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Scripts.View.Elevator
{
    public class ElevatorView : MonoBehaviour
    {
        private const float Duration = 0.2f;
        private const float IncreaseSize = 1.05f;

        private readonly List<Transform> _elevatorParts = new ();

        private void Awake()
        {
            var renderers = GetComponentsInChildren<SpriteRenderer>();
            var counterCanvas = GetComponentInChildren<Canvas>();

            _elevatorParts.Add(counterCanvas.transform);

            foreach (var spriteRenderer in renderers)
                _elevatorParts.Add(spriteRenderer.transform);
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
                part.DOScale(part.localScale * IncreaseSize, Duration).SetEase(Ease.OutSine).OnComplete(() =>
                    part.DOScale(0, Duration).SetEase(Ease.InSine));
            }
        }
    }
}