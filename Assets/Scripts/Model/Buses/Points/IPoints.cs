using UnityEngine;

namespace Scripts.Model.Buses.Points
{
    public interface IPoints
    {
        Vector3 StopPointer { get; }
        Vector3 BusStop { get; }
        Vector3 Finish { get; }
    }
}