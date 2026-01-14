using System;
using UnityEngine;

namespace Scripts.Model.Elevators.Data
{
    public class PositionCalculator : MonoBehaviour
    {
        [SerializeField] private TextAsset _jsonResource;
        [SerializeField] private Vector3 _busZeroPosition = new (-29.368f, 0.51f, 20.1967f);

        private Platform _platform;
        private ElevatorBackground _background;
        private ElevatorMultipliers _multipliers;
        private Vector3 _busShift;

        private void Awake()
        {
            _platform = GetComponentInChildren<Platform>();
            _background = GetComponentInChildren<ElevatorBackground>();

            if (_platform == null)
                throw new NullReferenceException(nameof(_platform));

            if (_background == null)
                throw new NullReferenceException(nameof(_background));

            _multipliers = JsonUtility.FromJson<ElevatorMultipliers>(_jsonResource.text);
        }

        public void Calculate(Vector3 busPosition)
        {
            _busShift = busPosition - _busZeroPosition;

            transform.localPosition = GetPosition(_multipliers.Position);
            transform.eulerAngles = GetPosition(_multipliers.Rotation);
            _background.transform.localPosition = GetPosition(_multipliers.BackgroundPosition);
            _platform.SetBottomPosition(GetPosition(_multipliers.BottomPlatformPosition));
        }

        private Vector3 GetPosition(ShiftData shiftData)
        {
            Vector3 shiftOnX = shiftData.MultiplierOnX * _busShift.x;
            Vector3 shiftOnY = shiftData.MultiplierOnY * _busShift.z;

            return shiftData.ZeroPosition + shiftOnX + shiftOnY;
        }
    }
}