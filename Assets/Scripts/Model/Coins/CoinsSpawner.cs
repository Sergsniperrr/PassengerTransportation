using System.Collections;
using UnityEngine;

public class CoinsSpawner : Spawner<Coin>
{
    [SerializeField] private Transform _target;

    private Coroutine _coroutine;
    private Coin _coin;

    public float MoveDuration { get; private set; } = 0.7f;

    protected override void Awake()
    {
        InitialPosition = transform.position;
        base.Awake();
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
        _coin = GetObject();
        _coin.Show(_target.position, false, false);
        MoveDuration = _coin.MoveDuration;
    }
}
