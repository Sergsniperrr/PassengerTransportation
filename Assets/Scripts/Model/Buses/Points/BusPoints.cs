using UnityEngine;

namespace Scripts.Model.Buses.Points
{
    public struct BusPoints : IPoints
    {
        public Vector3 StopPointer { get; private set; }
        public Vector3 BusStop { get; private set; }
        public Vector3 Finish { get; private set; }

        public void SetStopPointer(Vector3 point)
        {
            StopPointer = point;
        }

        public void SetStop(Vector3 point)
        {
            BusStop = point;
        }

        public void SetFinish(Vector3 point)
        {
            Finish = point;
        }
    }
}