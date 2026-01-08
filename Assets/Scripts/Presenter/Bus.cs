using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Presenter
{
    [RequireComponent(typeof(BusRouter))]
    [RequireComponent(typeof(ColorSetter))]
    [RequireComponent(typeof(PassengerReception))]
    [RequireComponent(typeof(BusView))]
    public class Bus : MonoBehaviour, ISenderOfFillingCompletion, IBusParameters, IParkingExitHandler
    {
        private readonly WaitForSeconds _waitOfCheckPassengers = new(0.01f);

        private BusRouter _router;
        private BusView _transformChanger;
        private Roof _roof;
        private ColorSetter _color;
        private PassengerReception _passengerReception;
        private Effects _effects;

        public event Action StartedMove;
        public event Action<Bus> LeftParkingLot;
        public event Action<Bus> FillingCompleted;

        public bool IsActive => _router.IsActive;
        public int StopIndex => _router.StopIndex;
        public int SeatsCount => _passengerReception.Count;
        public Material Material => _color.Material;
        public bool IsEmptySeat => _passengerReception.IsEmptySeat;
        public int FreeSeatsCount => _passengerReception.EmptySeatCount;

        private void Awake()
        {
            _router = GetComponent<BusRouter>();
            _roof = GetComponentInChildren<Roof>();
            _color = GetComponentInChildren<ColorSetter>();
            _passengerReception = GetComponent<PassengerReception>();
            _transformChanger = GetComponent<BusView>();

            if (_roof == null)
                throw new NullReferenceException(nameof(_roof));
        }

        public void InitializeData(IBusReceiver busStop, BusPointsCalculator navigator, Effects effects)
        {
            _router.InitializeData(busStop, navigator, effects);
            _effects = effects != null ? effects : throw new NullReferenceException(nameof(effects));
        }

        public void Activate() =>
            _router.Activate();

        public void Disable() =>
            _router.Disable();

        public void SetColor(Material material) =>
            _color.SetMateral(material);

        public void Run()
        {
            _router.StartMove();

            StartedMove?.Invoke();
        }

        public void EnableSwingEffect() =>
            _transformChanger.EnableSwingEffect();

        public void DisableSwingEffect() =>
            _transformChanger.DisableSwingEffect();

        public int ReserveFreePlace() =>
            _passengerReception.ReserveFreePlace();

        public void TakePassenger(Passenger passenger)
        {
            _passengerReception.TakePassenger(passenger);
            _transformChanger.BoardingEffect();
        }

        public void MoveOutFromBusStop()
        {
            _router.MoveOutFromBusStop();

            _router.WayFinished += Finish;
        }

        public void CompleteFilling()
        {
            _effects.PlayBusFillingComplete(StopIndex);

            FillingCompleted?.Invoke(this);
        }

        public void HandleParkingExit()
        {
            _router.BusStop.AddBusOnWayToStop(this, StopIndex);

            LeftParkingLot?.Invoke(this);
        }

        private void Finish()
        {
            _router.WayFinished -= Finish;

            StartCoroutine(DestroyAfterCheckPassengers());
        }

        private void ReleasePassengers()
        {
            var passengers = GetComponentsInChildren<Passenger>();

            foreach (Passenger passenger in passengers)
                if (passenger != null)
                    passenger.Finish();
                else
                    throw new NullReferenceException(nameof(passenger));
        }

        private IEnumerator DestroyAfterCheckPassengers()
        {
            while (TryGetComponent(out Passenger _))
            {
                ReleasePassengers();

                yield return _waitOfCheckPassengers;
            }

            Destroy(gameObject);
        }
    }
}
