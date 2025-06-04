using System;
using UnityEngine;

[RequireComponent(typeof(BusView))]
[RequireComponent(typeof(BusMover))]
public class TriggerHandler : MonoBehaviour
{
    private BusView _busView;
    private IStopOrMove _mover;

    public event Action BusCrashed;


    private void Awake()
    {
        _busView = GetComponent<BusView>();
        _mover = GetComponent<BusMover>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bus bus))
        {
            if (_mover.CanMove == false)
            {
                _busView.Swing();
                return;
            }

            _busView.ReleaseSparks();

            BusCrashed?.Invoke();
        }
    }
}
