using System;
using UnityEngine;

namespace Scripts.Model.Levels.SaveProgress.SerializeObjects
{
    [Serializable]
    public struct ElevatorData
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Vector3 Position { get; private set; }
        [field: SerializeField] public int Counter { get; private set; }

        public ElevatorData (string name, Vector3 position, int counter)
        {
            Name = name;
            Position = position;
            Counter = counter;
        }
    }
}
