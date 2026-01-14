using UnityEngine;

namespace Scripts.Model.Other
{
    public abstract class Spawner<T> : MonoBehaviour
        where T : SpawnableObject<T>
    {
        [SerializeField] private T _prefab;

        protected Vector3 InitialPosition;

        private ObjectPool<T> _pool;
        private T _spawnableObject;

        protected virtual void Awake()
        {
            _pool = new ObjectPool<T>(_prefab, transform, InitialPosition);
        }

        protected T GetObject()
        {
            _spawnableObject = _pool.GetObject();

            _spawnableObject.Died += PutObject;

            return _spawnableObject;
        }

        private void PutObject(T spawnableObject)
        {
            spawnableObject.Died -= PutObject;

            _pool.PutObject(spawnableObject);
        }
    }
}