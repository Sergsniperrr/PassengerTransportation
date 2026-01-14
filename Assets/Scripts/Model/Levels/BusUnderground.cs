using System;
using Scripts.Model.Buses;
using UnityEngine;

namespace Scripts.Model.Levels
{
    public class BusUnderground : IBusParameters
    {
        public BusUnderground(int seats, Material material)
        {
            SeatsCount = seats > 0 ? seats : throw new ArgumentOutOfRangeException(nameof(seats));
            Material = material != null ? material : throw new ArgumentNullException(nameof(material));
        }

        public int SeatsCount { get; private set; }
        public Material Material { get; private set; }

        public void SetColor(Material material)
        {
            Material = material != null ? material : throw new ArgumentNullException(nameof(material));
        }
    }
}