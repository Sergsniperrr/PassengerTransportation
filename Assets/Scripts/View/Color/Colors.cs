using UnityEngine;

public class Colors : MonoBehaviour
{
    [SerializeField] private Material[] _materals;

    public Material GetRandomColor() =>
        _materals[Random.Range(0, _materals.Length)];
}
