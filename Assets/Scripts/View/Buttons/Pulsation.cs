using DG.Tweening;
using UnityEngine;

namespace Scripts.View.Buttons
{
    public abstract class Pulsation : MonoBehaviour
    {
        [SerializeField] protected float Duration = 0.25f;

        private Tweener _tweener;

        private void OnEnable()
        {
            _tweener = StartAnimation();
        }

        protected virtual void OnDisable()
        {
            _tweener.Kill();
        }

        private void OnDestroy()
        {
            _tweener.Kill();
        }

        protected abstract Tweener StartAnimation();
    }
}