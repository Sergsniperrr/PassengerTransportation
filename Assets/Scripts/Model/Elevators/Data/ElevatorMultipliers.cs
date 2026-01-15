using UnityEngine;

namespace Scripts.Model.Elevators.Data
{
    [System.Serializable]
    public class ElevatorMultipliers
    {
        [field: SerializeField] public ShiftData Position { get; private set; }
        [field: SerializeField] public ShiftData Rotation { get; private set; }
        [field: SerializeField] public ShiftData BackgroundPosition { get; private set; }
        [field: SerializeField] public ShiftData BottomPlatformPosition { get; private set; }
        [field: SerializeField] public ShiftData CounterPosition { get; private set; }

        public void SetPosition(ShiftData value) =>
            Position = value;

        public void SetRotation(ShiftData value) =>
            Rotation = value;

        public void SetBackgroundPosition(ShiftData value) =>
            BackgroundPosition = value;

        public void SetBottomPlatformPosition(ShiftData value) =>
            BottomPlatformPosition = value;

        public void SetCounterPosition(ShiftData value) =>
            CounterPosition = value;
    }
}