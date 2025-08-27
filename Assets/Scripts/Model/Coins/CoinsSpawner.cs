using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CoinsPool))]
public class CoinsSpawner : MonoBehaviour
{
    [SerializeField] private Coin _prefab;
    [SerializeField] private Transform _target;

    private CoinsPool _pool;
    private Coroutine _coroutine;

    public float MoveDuration { get; private set; } = 0.7f;

    private void Awake()
    {
        _pool = GetComponent<CoinsPool>();
        _pool.InitializePrefab(_prefab);
    }

    public void StartSpawn(float period)
    {
        WaitForSeconds wait = new(period);

        _coroutine = StartCoroutine(SpawnEveryTime(wait));
    }

    public void StopSpawn()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator SpawnEveryTime(WaitForSeconds wait)
    {
        while (enabled)
        {
            Spawn();

            yield return wait;
        }
    }

    private void Spawn()
    {
        Coin coin = _pool.GetObject();
        coin.Show(_target.position, false);
        MoveDuration = coin.MoveDuration;

        coin.MoveComplete += ReturnCointToPool;
    }

    public void ReturnCointToPool(Coin coin)
    {
        coin.MoveComplete -= ReturnCointToPool;

        _pool.PutObject(coin);
    }
}
