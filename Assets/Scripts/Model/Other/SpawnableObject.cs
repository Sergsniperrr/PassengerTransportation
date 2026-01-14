using System;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Model.Other
{
    public class SpawnableObject<T> : MonoBehaviour
    {
        public event Action<T> Died;

        protected void Die(T self)
        {
            Died?.Invoke(self);
            transform.DOKill();
        }
    }
}