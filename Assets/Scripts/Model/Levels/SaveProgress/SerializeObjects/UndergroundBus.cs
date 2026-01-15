using System;
using UnityEngine;

namespace Scripts.Model.Levels.SaveProgress.SerializeObjects
{
    [Serializable]
    public struct UndergroundBus
    {
        public UndergroundBus(int seatsCount, int materialIndex)
        {
            const int MinSeatsCount = 4;

            SeatsCount = seatsCount >= MinSeatsCount
                ? seatsCount
                : throw new ArgumentOutOfRangeException(nameof(seatsCount));
            MaterialIndex = materialIndex;
        }

        [field: SerializeField] public int SeatsCount { get; private set; }
        [field: SerializeField] public int MaterialIndex { get; private set; }
    }
}