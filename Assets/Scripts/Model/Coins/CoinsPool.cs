using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinsPool : MonoBehaviour
{
    [SerializeField] private Vector3 _initialLocalPosition;

    private Coin _prefab;
    private Queue<Coin> _pool = new();
    private Coin _coin;

    private Vector3 _testInitialPosition = new(0f, 52f, 0f);

    public void InitializePrefab(Coin prefab) =>
        _prefab = prefab != null ? prefab : throw new ArgumentNullException(nameof(prefab));

    public Coin GetObject()
    {
        if (_pool.Count == 0)
        {
            _coin = Instantiate(_prefab, transform.position, Quaternion.identity);
            _coin.transform.SetParent(transform);
            _coin.InitializePosition(Vector3.zero);

            //_coin.DisableAnimator();

            return _coin;
        }

        _coin = _pool.Dequeue();
        //_coin.transform.localPosition = _initialLocalPosition;
        _coin.gameObject.SetActive(true);

        return _coin;
    }

    public void PutObject(Coin coin)
    {
        _pool.Enqueue(coin);
    }
}
