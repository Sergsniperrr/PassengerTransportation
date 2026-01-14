using Scripts.Presenters;
using UnityEngine;

namespace Scripts.Model.Elevators
{
    public class Platform : MonoBehaviour
    {
        [field: SerializeField] public Vector3 BottomPosition { get; private set; } = new (-20.08f, -9.73f, -47.38f);

        public Vector3 InitialBusPosition { get; private set; }

        public void SetBottomPosition(Vector3 position) =>
            BottomPosition = position;

        public Bus InitializeFirstBus()
        {
            const float MaxDistance = 0.5f;

            Vector3 shift = new (0f, -0.3f, 0f);
            Ray ray = new (transform.position + shift, Vector3.up);

            if (Physics.Raycast(ray, out RaycastHit hit, MaxDistance) == false)
                return null;

            if (hit.collider.TryGetComponent(out Bus bus) == false)
                return null;

            InitialBusPosition = bus.transform.position;
            bus.gameObject.SetActive(false);

            return bus;
        }
    }
}