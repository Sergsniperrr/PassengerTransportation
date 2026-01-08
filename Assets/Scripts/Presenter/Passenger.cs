using System;
using UnityEngine;

namespace Scripts.Presenter
{
    [RequireComponent(typeof(PassengerRouter))]
    [RequireComponent(typeof(ColorSetter))]
    public class Passenger : MonoBehaviour, ISenderOfGettingOnBus
    {
        private PassengerRouter _router;
        private ColorSetter _color;
        private Vector3 _initialPosition;
        private Transform _container;

        public event Action<Passenger> Died;
        public event Action<Passenger> GotOnBus;

        public Bus Bus { get; private set; }
        public int BusPlaceIndex { get; private set; }
        public bool IsFinishedMovement { get; private set; } = true;
        public Material Material => _color.Material;

        private void Awake()
        {
            _router = GetComponent<PassengerRouter>();
            _color = GetComponentInChildren<ColorSetter>();
            _initialPosition = transform.position;
        }

        public void InitializeQueueSize(int queueSize)
        {
            if (queueSize < 0)
                throw new ArgumentOutOfRangeException(nameof(queueSize));

            _router.InitializeQueueSize(queueSize, this);
        }

        public void InitializeContainer(Transform container) =>
            _container = container != null ? container : throw new NullReferenceException(nameof(container));

        public void SetColor(Material material) =>
            _color.SetMateral(material);

        public void IncrementCurrentIndex() =>
            _router.IncrementCurrentIndex();

        public void Finish()
        {
            ResetData();
            Died?.Invoke(this);
        }

        public void SetPlaceIndex(int index) =>
            _router.SetPlaceIndex(index);

        public void SendGettingOnBusAction()
        {
            GotOnBus?.Invoke(this);
        }

        public void GetOnBus(Bus bus)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));

            Bus = bus;
            _router.GetOnBus(bus);
        }

        public bool TryGetOnBus(Bus bus)
        {
            if (bus == null)
                throw new ArgumentNullException(nameof(bus));

            int failedIndex = -1;
            BusPlaceIndex = bus.ReserveFreePlace();

            if (BusPlaceIndex == failedIndex)
                return false;

            Bus = bus;
            _router.GetOnBus(bus);

            return true;
        }

        public void SpeedUp() =>
            _router.SpeedUp();

        public void ResetData()
        {
            transform.SetParent(_container);
            transform.position = _initialPosition;
            _router.ResetSpeed();
            Bus = null;
        }
    }
}
