using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : SpawnableObject<T>
{
    [SerializeField] private T _prefab;

    protected ObjectPool<T> Pool;
    protected Vector3 InitialPosition;

    private T _spawnableObject;

    protected virtual void Awake()
    {
        Pool = new(_prefab, transform, InitialPosition);
    }

    protected T GetObject()
    {
        _spawnableObject = Pool.GetObject();

        _spawnableObject.Died += PutObject;

        return _spawnableObject;
    }

    private void PutObject(T spawnableObject)
    {
        spawnableObject.Died -= PutObject;

        Pool.PutObject(spawnableObject);
    }
}
