using Scripts.Presenters;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [field: SerializeField] public Vector3 BottomPosition { get; private set; } = new(-20.08f, -9.73f, -47.38f);

    public Vector3 InitialBusPosition { get; private set; }

    public void SetBottomPosition(Vector3 position) =>
        BottomPosition = position;

    public Bus InitializeFirstBus()
    {
        float maxDistance = 0.5f;
        Vector3 shift = new(0f, -0.3f, 0f);
        Ray ray = new(transform.position + shift, Vector3.up);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.TryGetComponent(out Bus bus))
            {
                InitialBusPosition = bus.transform.position;
                bus.gameObject.SetActive(false);

                return bus;
            }
        }

        return null;
    }
}
