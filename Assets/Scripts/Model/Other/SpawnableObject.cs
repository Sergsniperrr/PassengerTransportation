using DG.Tweening;
using System;
using UnityEngine;

public class SpawnableObject<T> : MonoBehaviour
{
    public event Action<T> Died;

    protected void Die(T self)
    {
        Died?.Invoke(self);
        transform.DOKill();
    }
}
