using System;
using UnityEngine;

[RequireComponent(typeof(BusMover))]
public class TriggerHandler : MonoBehaviour
{
    private BusMover _mover;
    private bool _canCrash = false;

    public event Action BusStopTriggered;
    public event Action WayFinished;
    public event Action BusCrashed;

    private void Awake()
    {
        _mover = GetComponent<BusMover>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Pointer pointer))
        {
            HandleTriggerPoint(pointer);
            _canCrash = false;
        }

        if (other.TryGetComponent(out EndPoint _))
        {
            WayFinished?.Invoke();
        }

        if (other.TryGetComponent(out BusStop busStop))
        {
            Vector3 position = transform.position;
            position.z = other.transform.position.z;
            transform.position = position;

            BusStopTriggered?.Invoke();
            _canCrash = false;
        }

        if (_canCrash == false)
        {
            return;
        }

        if (other.TryGetComponent(out Bus bus))
        {
            BusCrashed?.Invoke();
        }
    }

    public void EnableCrash() =>
        _canCrash = true;

    public void DisableCrash() =>
        _canCrash = false;

    private void HandleTriggerPoint(Pointer trigger)
    {
        float triggerRotation = 90f;
        Vector3 newPosition = transform.position;
        Vector3 target = trigger.Target.position;

        if (trigger.transform.rotation.eulerAngles.y != triggerRotation)
            newPosition.x = trigger.transform.position.x;
        else
            newPosition.z = trigger.transform.position.z;

        target.y = transform.position.y;
        transform.position = newPosition;

        _mover.ChangeDirection(target);
    }
}
