using UnityEngine;
using DG.Tweening;

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
        _tweener.Kill(false);
    }

    private void OnDestroy()
    {
        _tweener.Kill(false);
    }

    protected abstract Tweener StartAnimation();
}
