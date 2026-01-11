using System;
using DG.Tweening;
using Scripts.Model.Buses;
using Scripts.Sounds;
using UnityEngine;

namespace Scripts.View.Buses
{
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(PassengerReception))]
    public class BusView : MonoBehaviour
    {
        private const int HalfDevider = 2;

        private readonly float _swingAmplitude = 12f;
        private readonly float _swingDuration = 0.15f;
        private readonly float _pulsationDuration = 0.07f;
        private readonly Vector3 _pulsationScale = new(1.2f, 0.5f, 0.9f);
        private readonly Vector3 _sizeAtStop = new(1.05f, 0.85f, 0.9f);

        private Roof _roof;
        private ParticleSystem _smoke;
        private Effects _effects;
        private Tween _tween;
        private Vector3 _initialRotation;
        private Vector3 _sparksShiftPosition;
        private PassengerReception _passengerReception;
        private bool _isSwingEnabled = true;

        private void Awake()
        {
            _smoke = GetComponent<ParticleSystem>();
            _passengerReception = GetComponent<PassengerReception>();
            _roof = GetComponentInChildren<Roof>();

            if (_roof == null)
                throw new NullReferenceException(nameof(_roof));

            _initialRotation = transform.rotation.eulerAngles;
        }

        public void InitializeData(Effects effects)
        {
            _effects = effects != null ? effects : throw new NullReferenceException(nameof(effects));
            _sparksShiftPosition = CalculateSparksPosition();
        }

        public void GrowToHalfSizeAtStop()
        {
            Vector3 scale = transform.localScale;
            transform.localScale = (_sizeAtStop - scale) / HalfDevider + scale;
        }

        public void GrowToFullSizeAtStop() =>
            transform.localScale = _sizeAtStop;

        public void DisableRoof() =>
            _roof.gameObject.SetActive(false);

        public void EnableSwingEffect() =>
            _isSwingEnabled = true;

        public void DisableSwingEffect() =>
            _isSwingEnabled = false;

        public void BoardingEffect()
        {
            _effects.PlayBoardingBus();
            transform.localScale = _pulsationScale;

            _tween.Kill();
            _tween = transform.DOScale(_sizeAtStop, _pulsationDuration);
        }

        public void EnableSmoke() =>
            _smoke.Play();

        public void DisableSmoke() =>
            _smoke.Stop();

        public void Swing()
        {
            if (_isSwingEnabled == false)
                return;

            Vector3 forwardSwing = _initialRotation;
            Vector3 backwardSwing = _initialRotation;

            forwardSwing.z = _swingAmplitude;
            backwardSwing.z = _swingAmplitude * -1;

            DOTween.Sequence()
                .Append(transform.DOLocalRotate(forwardSwing, _swingDuration / HalfDevider))
                .Append(transform.DOLocalRotate(backwardSwing, _swingDuration))
                .Append(transform.DOLocalRotate(_initialRotation, _swingDuration));
        }

        public void PlayCrashEffect() =>
            _effects.PlayCrash(_sparksShiftPosition, transform);

        private Vector3 CalculateSparksPosition()
        {
            Vector3 sparksShift = new(0f, 0.39f, 0f);
            int trippleDevider = 3;
            float minDistanceToSparks = 0.85f;
            float stepSize = 0.175f;
            int stepsCount = _passengerReception.Count / trippleDevider - 1;
            float distanseToSparks = minDistanceToSparks + (stepSize * stepsCount);

            sparksShift.z = distanseToSparks;

            return sparksShift;
        }
    }
}
