using System;
using UnityEngine;

public class MouseInputHandler : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    private const int LeftMouseButton = 0;

    private Ray _ray;
    private RaycastHit _hitInfo;
    private Collider _target;

    public event Action<Bus> BusSelected;

    private void Update()
    {
        _target = OnMouseClick();

        if (_target == null)
            return;

        if (_target.TryGetComponent(out Bus bus))
            BusSelected?.Invoke(bus);
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
