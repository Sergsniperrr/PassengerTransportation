using System;
using UnityEngine;

namespace Scripts.Model.Levels
{
    [Serializable]
    public struct BusData
    {
        public BusData(int seatsCount, Vector3 position, Quaternion rotation)
        {
            SeatsCount = seatsCount;
            Position = position;
            Rotation = rotation;
        }

        [field: SerializeField] public int SeatsCount { get; private set; }
        [field: SerializeField] public Vector3 Position { get; private set; }
        [field: SerializeField] public Quaternion Rotation { get; private set; }
    }
}