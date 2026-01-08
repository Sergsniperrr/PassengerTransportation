using System.Collections.Generic;
using Scripts.Presenter;
using UnityEngine;

public class PassengerPool : MonoBehaviour
{
    [SerializeField] private Passenger _prefab;

    private readonly Queue<Passenger> _pool = new();

    private Passenger _passenger;
    private Vector3 _initialScale;

    private void Awake()
    {
        _initialScale = _prefab.transform.localScale;
    }

    public Passenger GetObject()
    {
        if (_pool.Count == 0)
        {
            _passenger = Instantiate(_prefab, transform.position, Quaternion.identity);
            _passenger.transform.SetParent(transform);
            _passenger.InitializeContainer(transform);

            return _passenger;
        }

        _passenger  = _pool.Dequeue();
        _passenger.gameObject.SetActive(true);
        _passenger.transform.localScale = _initialScale;

        return _passenger;
    }

    public void PutObject(Passenger passenger)
    {
        _pool.Enqueue(passenger);

        passenger.gameObject.SetActive(false);
    }
}
