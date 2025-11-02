using System;
using UnityEngine;

[Serializable]
public struct VisibleBus
{
    [field: SerializeField] public BusData BusData { get; private set; }
    [field: SerializeField] public int ColorIndex { get; private set; }

    public VisibleBus (BusData data, int colorIndex)
    {
        BusData = data;
        ColorIndex = colorIndex;
    }
}
