using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectPool<T> : Component where T : SpawnableObject<T>
{
    private T _prefab;
    private Transform _parent;
    private Vector3 _initialPosition;
    private Queue<T> _pool = new();
    private T _coin;

    public ObjectPool(T prefab, Transform parent, Vector3 initialPosition)
    {
        _prefab = prefab ?? throw new ArgumentNullException(nameof(prefab));
        _parent = parent != null ? parent : throw new ArgumentNullException(nameof(_parent));
        _initialPosition = initialPosition;
    }

    public T GetObject()
    {
        if (_pool.Count == 0)
        {
            _coin = Instantiate(_prefab);
            _coin.transform.SetParent(_parent);
        }
        else
        {
            _coin = _pool.Dequeue();
            _coin.gameObject.SetActive(true);
        }

        _coin.transform.SetLocalPositionAndRotation(_initialPosition, Quaternion.identity);

        return _coin;
    }

    public void PutObject(T coin)
    {
        _pool.Enqueue(coin);
        coin.gameObject.SetActive(false);
    }
}
