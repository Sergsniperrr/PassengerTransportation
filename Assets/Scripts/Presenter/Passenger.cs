using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PassengerMover))]
[RequireComponent(typeof(ColorSetter))]
public class Passenger : MonoBehaviour
{
    private PassengerMover _mover;
    private ColorSetter _color;

    public Material Material => _color.Material;

    private void Awake()
    {
        _mover = GetComponent<PassengerMover>();
        _color = GetComponentInChildren<ColorSetter>();
    }

    public void SetColor(Material material) =>
        _color.SetMateral(material);

    public void MoveTo(Vector3 target) =>
        _mover.MoveTo(target);
}
