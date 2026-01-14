using Scripts.Model.BusStops;
using UnityEngine;

namespace Scripts.Model.Passengers
{
    [RequireComponent(typeof(PassengerAnimator))]
    public class PassengerMover : MonoBehaviour
    {
        private const int FailedIndex = -1;
        private const float SpeedMultiplier = 2f;

        [SerializeField] private float _speed;

        private ISenderOfGettingOnBus _sender;
        private Vector3[] _positions;
        private PassengerAnimator _animator;
        private int _currentPosition;
        private float _initialSpeed;
        private bool _isInQueue;

        private void Awake()
        {
            _animator = GetComponent<PassengerAnimator>();
            _initialSpeed = _speed;
        }

        private void Update()
        {
            Move();
        }

        public void InitializeData(ISenderOfGettingOnBus sender) =>
            _sender = sender;

        public void SetRout(Vector3[] positions, bool isInQueue = true)
        {
            _positions = positions;
            SetPlaceIndex(0);
            _isInQueue = isInQueue;
        }

        public void IncrementCurrentIndex()
        {
            if (_currentPosition < _positions.Length - 1)
            {
                SetPlaceIndex(_currentPosition + 1);
            }
        }

        public void SetPlaceIndex(int index)
        {
            _currentPosition = index;
            transform.LookAt(_positions[_currentPosition]);

            _animator.Move();
        }

        public void SpeedUp() =>
            _speed *= SpeedMultiplier;

        public void ResetSpeed() =>
            _speed = _initialSpeed;

        private void Move()
        {
            if (_currentPosition == FailedIndex)
                return;

            if (_positions.Length > 0 && transform.position != _positions[_currentPosition])
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, _positions[_currentPosition], _speed * Time.deltaTime);
            }
            else if (_isInQueue == false && _currentPosition < _positions.Length - 1)
            {
                SetPlaceIndex(_currentPosition + 1);
            }
            else
            {
                if (_isInQueue == false && _currentPosition == _positions.Length - 1)
                {
                    _sender.SendGettingOnBusAction();
                    _currentPosition = FailedIndex;
                }

                _animator.Stop();
            }
        }
    }
}