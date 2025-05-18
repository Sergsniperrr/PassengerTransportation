using System;
using UnityEngine;

public class MouseInputHandler : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    private const int LeftMouseButton = 0;

    private readonly float _updateDelay = 0.3f;

    private Ray _ray;
    private RaycastHit _hitInfo;
    private Collider _target;
    private float _updateCounter;

    public event Action<Bus> BusSelected;

    private void Update()
    {
        if (_updateCounter > 0)
        {
            _updateCounter -= Time.deltaTime;
            return;
        }

        _target = OnMouseClick();

        if (_target == null)
            return;

        if (_target.TryGetComponent(out Bus bus))
        {
            if (bus.IsActive)
            {
                BusSelected?.Invoke(bus);
                _updateCounter = _updateDelay;
            }
        }
    }

    private Collider OnMouseClick()
    {
        if (Input.GetMouseButtonDown(LeftMouseButton))
        {
            _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hitInfo))
                return _hitInfo.collider;
        }

        return null;
    }
}
