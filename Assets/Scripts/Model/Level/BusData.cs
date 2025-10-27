using System;
using UnityEngine;

[Serializable]
public struct BusData
{
    [field: SerializeField] public int SeatsCount { get; private set; }
    [field: SerializeField] public Vector3 Position { get; private set; }
    [field: SerializeField] public Quaternion Rotation { get; private set; }

    public Material Material => throw new NotImplementedException();

    public BusData(int seatsCount, Vector3 position, Quaternion rotation)
    {
        SeatsCount = seatsCount;
        Position = position;
        Rotation = rotation;
    }
}
