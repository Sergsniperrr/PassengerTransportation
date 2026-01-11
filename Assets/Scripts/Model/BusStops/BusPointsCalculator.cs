using System;
using Scripts.Model.Buses.Points;
using UnityEngine;

namespace Scripts.Model.BusStops
{
    public class BusPointsCalculator : MonoBehaviour
    {
        private const float PointerShiftOnX = 1.65f;
        private const float PointerShiftOnZ = 2.82f;
        private const float FinishPositionX = -36.5f;
    
        [SerializeField] private Transform[] _points;

        public BusPoints CalculatePoints(int stopIndex, float positionY)
        {
            BusPoints points = new();

            points.SetStopPointer(CalculatePointerCoordinate(stopIndex, positionY));
            points.SetStop(CalculateStopCoordinate(stopIndex, positionY));

            var finish = points.StopPointer;
            finish.x = FinishPositionX;
            points.SetFinish(finish);

            return points;
        }

        private Vector3 CalculatePointerCoordinate(int stopIndex, float positionY)
        {
            if (stopIndex < 0 || stopIndex >= _points.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(stopIndex));
            }

            Vector3 position = Vector3.zero;

            position.x = _points[stopIndex].position.x + PointerShiftOnX;
            position.z = _points[stopIndex].position.z + PointerShiftOnZ;
            position.y = positionY;

            return position;
        }

        private Vector3 CalculateStopCoordinate(int stopIndex, float positionY)
        {
            if (stopIndex < 0 || stopIndex >= _points.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(stopIndex));
            }

            Vector3 position = _points[stopIndex].position;
            position.y = positionY;

            return position;
        }
    }
}