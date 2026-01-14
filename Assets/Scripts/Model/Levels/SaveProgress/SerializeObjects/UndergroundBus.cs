using System;
using UnityEngine;

namespace Scripts.Model.Levels.SaveProgress.SerializeObjects
{
    [Serializable]
    public struct UndergroundBus
    {
        [field: SerializeField] public int SeatsCount { get; private set; }
        [field: SerializeField] public int MaterialIndex { get; private set; }

        public UndergroundBus(int seatsCount, int materialIndex)
        {
            int minSeatsCount = 4;

            SeatsCount = seatsCount >= minSeatsCount ? seatsCount : throw new ArgumentOutOfRangeException(nameof(seatsCount));
            MaterialIndex = materialIndex;
        }
    }
}
