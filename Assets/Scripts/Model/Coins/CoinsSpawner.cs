using System.Collections;
using UnityEngine;

public class CoinsSpawner : Spawner<Coin>
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _spawnInterval = 0.08f;

    private Coroutine _coroutine;
    private Coin _coin;
    private WaitForSeconds _waitForSpawn;

    public float MoveDuration { get; private set; } = 0.7f;

    protected override void Awake()
    {
        InitialPosition = transform.position;
        _waitForSpawn = new(_spawnInterval);
        base.Awake();
    }

    public void StartSpawn() =>
        _coroutine = StartCoroutine(SpawnEveryTime());

    public void StopSpawn()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator SpawnEveryTime()
    {
        while (enabled)
        {
            Spawn();

            yield return _waitForSpawn;
        }
    }

    private void Spawn()
    {
        _coin = GetObject();
        _coin.Show(_target.position, false, false);
        MoveDuration = _coin.MoveDuration;
    }
}
