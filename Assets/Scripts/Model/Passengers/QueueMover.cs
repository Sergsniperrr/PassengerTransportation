using System.Collections.Generic;
using Scripts.Presenters;
using UnityEngine;

namespace Scripts.Model.Passengers
{
    public class QueueMover : MonoBehaviour
    {
        public void IncrementPositions(List<Passenger> queue)
        {
            foreach (Passenger passenger in queue)
            {
                passenger.IncrementCurrentIndex();
            }
        }

        public void StartMovePassengers(List<Passenger> passengers)
        {
            for (int i = 0; i < passengers.Count; i++)
            {
                passengers[i].SetPlaceIndex(passengers.Count - i - 1);
            }
        }
    }
}