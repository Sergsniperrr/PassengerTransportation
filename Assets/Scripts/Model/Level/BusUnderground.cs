using System;
using UnityEngine;

public class BusUnderground : IBusParameters
{
    public int SeatsCount { get; private set; }
    public Material Material { get; private set; }

    public BusUnderground(int seats, Material material)
    {
        SeatsCount = seats > 0 ? seats : throw new ArgumentOutOfRangeException(nameof(seats));
        Material = material != null ? material : throw new ArgumentNullException(nameof(material));
    }

    public void SetColor(Material material)
    {
        Material = material != null ? material : throw new ArgumentNullException(nameof(material));
    }
}
