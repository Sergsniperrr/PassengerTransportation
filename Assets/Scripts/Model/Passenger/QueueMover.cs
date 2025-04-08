using System.Collections;
using UnityEngine;

public class QueueMover : MonoBehaviour
{
    private readonly int _rotaryIndex = 10;
    private readonly float _stepSize = 0.5f;
    private readonly float _reverseDerection = -1f;
    private readonly float _outPointShiftOnZ = 0.7f;

    private Vector3[] _coordinates;
    private Vector3 _outPoint;

    public void InitializeData(Vector3 zeroPosition, int elementsCount)
    {
        Vector3 shift = Vector3.zero;
        _coordinates = new Vector3[elementsCount];


        for (int i = 0; i < _rotaryIndex; i++)
        {
            shift.z = i * _stepSize;
            _coordinates[i] = zeroPosition + shift;
        }

        for (int i = _rotaryIndex; i < elementsCount; i++)
        {
            shift.x = (i - _rotaryIndex + 1) * _stepSize * _reverseDerection;
            _coordinates[i] = zeroPosition + shift;
        }

        CalculateOutPoint();
    }

    public void MoveOneStep(Passenger[] queue)
    {
        for (int i = 0; i < queue.Length; i++)
        {
            if (queue[i] != null)
                queue[i].MoveTo(_coordinates[i]);
        }
    }

    public void StartMove(Passenger[] queue) =>
        StartCoroutine(StartMoveCoroutine(queue));

    public void MoveOutPassenger(Passenger passenger) =>
        passenger.MoveTo(_outPoint);

    public IEnumerator StartMoveCoroutine(Passenger[] queue)
    {
        int rotaryIndex = _rotaryIndex - 1;
        float delay = 0.045f;
        WaitForSeconds wait = new(delay);

        for (int i = queue.Length - 1; i >= 0; i--)
        {
            if (queue[i] == null)
                continue;

            if (i > rotaryIndex)
                queue[i].MoveTo(_coordinates[rotaryIndex]);

            queue[i].MoveTo(_coordinates[i]);

            yield return wait;
        }
    }

    private void CalculateOutPoint()
    {
        _outPoint = _coordinates[^1];
        _outPoint.z = _outPointShiftOnZ;
    }
}
