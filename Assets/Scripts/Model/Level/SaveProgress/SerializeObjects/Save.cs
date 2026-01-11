using System;
using UnityEngine;

namespace Scripts.Model.Level.SaveProgress.SerializeObjects
{
    [Serializable]
    public struct Save
    {
        [field: SerializeField] public bool IsNewSave { get; private set; }
        [field: SerializeField] public int MoneyBuffer { get; private set; }
        [field: SerializeField] public int[] Colors { get; private set; }
        [field: SerializeField] public VisibleBus[] VisualBuses { get; private set; }
        [field: SerializeField] public BusStopSpot[] BusStopSpots { get; private set; }
        [field: SerializeField] public ElevatorData[] Elevators { get; private set; }
        [field: SerializeField] public UndergroundBus[] UndergroundBuses { get; private set; }

        public Save(int moneyBuffer, int[] colors, VisibleBus[] visualBuses)
        {
            IsNewSave = true;
            MoneyBuffer = moneyBuffer >= 0 ? moneyBuffer : throw new ArgumentOutOfRangeException(nameof(moneyBuffer));
            Colors = colors;

            VisualBuses = visualBuses;
            BusStopSpots = null;
            Elevators = null;
            UndergroundBuses = null;
        }

        public void SetVisualBuses(VisibleBus[] buses) =>
            VisualBuses = buses;

        public void SetElevators(ElevatorData[] elevators) =>
            Elevators = elevators;

        public void SetUndergroundBuses(UndergroundBus[] buses) =>
            UndergroundBuses = buses;

        public void SetBusStopSpots(BusStopSpot[] spots) =>
            BusStopSpots = spots;

        public void Reset() =>
            IsNewSave = false;
    }
}
