using UnityEngine;

namespace Scripts.Model.Elevators.Data
{
    [System.Serializable]
    public struct ShiftData
    {
        public ShiftData(Vector3 x, Vector3 y, Vector3 zeroPosition)
        {
            MultiplierOnX = x;
            MultiplierOnY = y;
            ZeroPosition = zeroPosition;
        }

        [field: SerializeField] public Vector3 MultiplierOnX { get; private set; }
        [field: SerializeField] public Vector3 MultiplierOnY { get; private set; }
        [field: SerializeField] public Vector3 ZeroPosition { get; private set; }
    }
}