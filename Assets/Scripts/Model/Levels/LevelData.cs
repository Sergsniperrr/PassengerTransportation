using System;
using System.Linq;
using UnityEngine;

namespace Scripts.Model.Levels
{
    [Serializable]
    public class LevelData
    {
        [field: SerializeField] public BusData[] Buses { get; private set; }

        public void AddBusesData(BusData[] buses)
        {
            Buses = buses.ToArray() ?? throw new ArgumentNullException(nameof(buses));
        }
    }
}