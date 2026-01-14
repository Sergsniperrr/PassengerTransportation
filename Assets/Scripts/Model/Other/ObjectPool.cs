using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Model.Other
{
    public class ObjectPool<T> : Component
        where T : SpawnableObject<T>
    {
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly Vector3 _initialPosition;
        private readonly Queue<T> _pool = new ();

        private T _object;

        public ObjectPool(T prefab, Transform parent, Vector3 initialPosition)
        {
            _prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
            _parent = parent != null ? parent : throw new ArgumentNullException(nameof(parent));
            _initialPosition = initialPosition;
        }

        public T GetObject()
        {
            if (_pool.Count == 0)
            {
                _object = Instantiate(_prefab, _parent, true);
            }
            else
            {
                _object = _pool.Dequeue();
                _object.gameObject.SetActive(true);
            }

            _object.transform.SetLocalPositionAndRotation(_initialPosition, Quaternion.identity);

            return _object;
        }

        public void PutObject(T coin)
        {
            _pool.Enqueue(coin);
            coin.gameObject.SetActive(false);
        }
    }
}