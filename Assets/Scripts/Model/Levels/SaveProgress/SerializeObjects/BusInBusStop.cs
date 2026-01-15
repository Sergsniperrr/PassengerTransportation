using System;
using UnityEngine;

namespace Scripts.Model.Levels.SaveProgress.SerializeObjects
{
    [Serializable]
    public struct BusInBusStop
    {
        [field: SerializeField] public VisibleBus VisibleBus { get; }
        [field: SerializeField] public int PassengersCount { get; }

        public BusInBusStop(VisibleBus bus, int passengersCount)
        {
            VisibleBus = bus;
            PassengersCount = passengersCount;
        }
    }
}