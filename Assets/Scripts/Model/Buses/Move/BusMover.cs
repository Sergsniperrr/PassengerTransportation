using Scripts.Model.Buses.Points;
using Scripts.Sounds;
using UnityEngine;

namespace Scripts.Model.Buses.Move
{
    [RequireComponent(typeof(PointsHandler))]
    [RequireComponent(typeof(BusEngineSound))]
    public class BusMover : MonoBehaviour, IMoveCorrector, IStopOrMove
    {
        private const float _directionForward = 1f;

        [SerializeField] private float _speed;

        private PointsHandler _pointsHandler;
        private BusEngineSound _audio;
        private IPoints _points;
        private Vector3 _velocity = Vector3.forward;
        private Vector3 _target = Vector3.zero;
        private Vector3 _initialPlace;
        private int _stopIndex;
        private float _direction = 1f;
        private bool _isMoveForward = true;

        public bool CanMove { get; private set; }
        public bool IsFilled { get; private set; }

        private void Awake()
        {
            _velocity.z = _speed;
            _initialPlace = transform.position;
            _pointsHandler = GetComponent<PointsHandler>();
            _audio = GetComponent<BusEngineSound>();
        }

        private void Update()
        {
            Move();
        }

        public void InitializeData(int stopIndex)
        {
            _pointsHandler.InitializePoints(stopIndex);
            _points = _pointsHandler.Points;
            _stopIndex = stopIndex;
            DisableForwardMovement();
            SetTarget(_points.StopPointer);
        }

        public void EnableMovement()
        {
            CanMove = true;
            _audio.PlaySound();
        }

        public void EnableForwardMovement()
        {
            _isMoveForward = true;
        }

        public void DisableForwardMovement()
        {
            _isMoveForward = false;
        }

        public void DisableMovement()
        {
            CanMove = false;
            _audio.StopSound();
        }

        public void ResetTarget() =>
            _target = Vector3.zero;

        public void ChangeDirection(Vector3 target)
        {
            target.y = transform.position.y;
            transform.LookAt(target);
            _direction = _directionForward;
            CanMove = true;
        }

        public void GoBackwardsToPoint()
        {
            _isMoveForward = false;
            SetTarget(_initialPlace, false);
        }

        public void SetTarget(Vector3 target, bool canLookAtTarget = true)
        {
            if (canLookAtTarget)
                transform.LookAt(target);

            _target = target;
        }

        public void MoveOutFromBusStop()
        {
            IsFilled = true;
            SetTarget(_points.StopPointer, false);
            CanMove = true;
            _audio.MoveOut(_stopIndex);
        }

        private void Move()
        {
            if (CanMove == false)
                return;

            if (_isMoveForward)
            {
                transform.Translate(_direction * Time.deltaTime * _velocity);
                return;
            }

            if (transform.position == _target)
            {
                _pointsHandler.HandlePoints(_target);
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, _target, _speed * Time.deltaTime);
        }
    }
}