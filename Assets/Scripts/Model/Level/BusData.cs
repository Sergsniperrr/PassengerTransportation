using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BusData
{
    [field: SerializeField] public int SeatsCount { get; private set; }
    [field: SerializeField] public Vector3 Position { get; private set; }
    [field: SerializeField] public Quaternion Rotation { get; private set; }

    public void SetSeatsCount(int count)
    {
        SeatsCount = count > 0 ? count : throw new ArgumentOutOfRangeException(nameof(count));
    }

    public void SetPosition(Vector3 position)
    {
        Position = position != Vector3.zero ? position : throw new ArgumentException(nameof(position));
    }

    public void SetRotation(Quaternion rotation)
    {
        Rotation = rotation;
    }
}
