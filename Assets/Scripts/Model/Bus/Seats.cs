using UnityEngine;

public class Seats : MonoBehaviour
{
    [field: SerializeField] public int Count { get; private set; }
    [field: SerializeField] public Vector3 FirstPlaceCoordinate { get; private set; }
    [field: SerializeField] public float SideInterval { get; private set; }
    [field: SerializeField] public float BackInterval { get; private set; }
}
