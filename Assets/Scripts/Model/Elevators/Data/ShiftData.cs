using System;
using UnityEngine;

[System.Serializable]
public struct ShiftData
{
    [field: SerializeField] public Vector3 MultiplierOnX { get; private set; }
    [field: SerializeField] public Vector3 MultiplierOnY { get; private set; }
    [field: SerializeField] public Vector3 ZeroPosition { get; private set; }

    public ShiftData(Vector3 x, Vector3 y, Vector3 zeroPosition)
    {
        MultiplierOnX = x;
        MultiplierOnY = y;
        ZeroPosition = zeroPosition;
    }
}
