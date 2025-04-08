using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Colors))]
public class Level : MonoBehaviour
{
    private Colors _colorHandler;
    private Bus[] _buses;

    private void Awake()
    {
        _colorHandler = GetComponent<Colors>();
        _buses = GetComponentsInChildren<Bus>();
    }

    public void SetBusesRandomColor()
    {
        foreach (Bus bus in _buses)
            bus.SetColor(_colorHandler.GetRandomColor());
    }
}
