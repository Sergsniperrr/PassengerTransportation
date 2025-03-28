using UnityEngine;

public class BusMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Collider _bufferCollider;
    private Vector3 _velocity = Vector3.forward;
    private bool _canMove = false;

    private void Awake()
    {
        _velocity.z = _speed;
    }

    private void Update()
    {
        if (_canMove)
            transform.Translate(_velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == _bufferCollider)
            return;

        if (other.TryGetComponent(out Pointer pointer))
        {
            HandleTriggerPoint(pointer);
        }

        if (other.TryGetComponent(out EndPoint _))
        {
            Destroy(gameObject);
        }
    }

    private void Stop() =>
        _canMove = false;

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

        transform.LookAt(target);
    }

    public void Run() =>
        _canMove = true;
}
