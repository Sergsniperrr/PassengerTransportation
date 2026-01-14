using System;
using Scripts.Presenters;
using UnityEngine;

namespace Scripts.Input
{
    public class MouseInputHandler : MonoBehaviour
    {
        private const int LeftMouseButton = 0;
        private const float UpdateDelay = 0.3f;

        [SerializeField] private Camera _mainCamera;

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

            if (_target.TryGetComponent(out Bus bus) == false)
                return;

            if (bus.IsActive == false)
                return;

            BusSelected?.Invoke(bus);
            _updateCounter = UpdateDelay;
        }

        private Collider OnMouseClick()
        {
            if (UnityEngine.Input.GetMouseButtonDown(LeftMouseButton))
            {
                _ray = _mainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);

                if (Physics.Raycast(_ray, out _hitInfo))
                {
                    return _hitInfo.collider;
                }
            }

            return null;
        }
    }
}