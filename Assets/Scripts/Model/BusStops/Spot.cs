using System;
using Scripts.Presenters;

namespace Scripts.Model.BusStops
{
    public struct Spot
    {
        private Bus _bus;

        public Spot(bool isFree = true)
        {
            IsFree = isFree;
            _bus = null;
            BusOnWayToStop = null;
            BusAtBusStop = null;
        }

        public bool IsFree { get; private set; }
        public Bus BusOnWayToStop { get; private set; }
        public Bus BusAtBusStop { get; private set; }

        public void Reserve() =>
            IsFree = false;

        public void SetBusOnWayToStop(Bus bus)
        {
            _bus = bus != null ? bus : throw new ArgumentNullException(nameof(bus));
            BusOnWayToStop = _bus;
        }

        public void PlaceBusInStop()
        {
            BusOnWayToStop = null;
            BusAtBusStop = _bus;
        }

        public void ReleaseStop()
        {
            CancelReservation();
            BusAtBusStop = null;
            _bus = null;
        }

        private void CancelReservation() =>
            IsFree = true;
    }
}