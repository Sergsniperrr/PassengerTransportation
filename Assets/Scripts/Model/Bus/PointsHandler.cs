using System;
using UnityEngine;

[RequireComponent(typeof(BusMover))]
[RequireComponent(typeof(BusView))]
public class PointsHandler : MonoBehaviour
{
    private BusPointsCalculator _calculator;
    private BusView _busView;
    private Vector3 _initialPlace;
    private IMoveCorrector _mover;

    public event Action ArrivedToBusStop;
    public event Action ReturnedToInitialPlace;
    public event Action EndpointArrived;

    public BusPoints Points { get; private set; }

    private void Awake()
    {
        _initialPlace = transform.position;
        _mover = GetComponent<BusMover>();
        _busView = GetComponent<BusView>();
    }

    public void InitializeData(BusPointsCalculator calculator)
    {
        _calculator = calculator != null ? calculator : throw new ArgumentNullException(nameof(calculator));
    }

    public void InitializePoints(int stopIndex)
    {
        Points = _calculator.CalculatePoints(stopIndex, transform.position.y);
    }

    public void HandlePoints(Vector3 target)
    {
        if (target == Vector3.zero)
            return;

        if (target == _initialPlace)
        {
            _mover.EnableForwardMovement();
            _mover.DisableMovement();
            _mover.ResetTarget();
            _busView.DisableSmoke();
            ReturnedToInitialPlace?.Invoke();
        }
        else if (target == Points.StopPointer)
        {
            if (_mover.IsFilled)
            {
                _mover.SetTarget(Points.Finish);
            }
            else
            {
                _mover.DisableForwardMovement();
                _mover.SetTarget(Points.BusStop);
            }
        }
        else if (target == Points.BusStop)
        {
            _busView.GrowToFullSizeAtStop();
            _busView.DisableRoof();
            _busView.DisableSmoke();
            _mover.DisableMovement();
            _mover.ResetTarget();

            ArrivedToBusStop?.Invoke();
        }
        else if (target == Points.Finish)
        {
            EndpointArrived?.Invoke();
        }
    }
}
