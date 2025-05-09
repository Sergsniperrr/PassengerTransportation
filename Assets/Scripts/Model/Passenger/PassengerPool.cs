using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerPool : MonoBehaviour
{
    [SerializeField] private Passenger _prefab;

    private readonly Queue<Passenger> _pool = new();

    private Passenger _passenger;

    public Passenger GetObject()
    {
        if (_pool.Count == 0)
        {
            _passenger = Instantiate(_prefab, transform.position, Quaternion.identity);
            _passenger.transform.SetParent(transform);

            return _passenger;
        }

        _passenger  = _pool.Dequeue();
        _passenger.gameObject.SetActive(true);
        _passenger.AllowMovement();
        _passenger.ResetSpeed();
        _passenger.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

        return _passenger;
    }

    public void PutObject(Passenger passenger)
    {
        passenger.transform.SetParent(transform);
        _pool.Enqueue(passenger);

        passenger.gameObject.SetActive(false);
    }
}
