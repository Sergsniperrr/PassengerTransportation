using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuePositionsCalculator : MonoBehaviour
{
    private List<Vector3> _positions = new();
    private float _stepSize = 0.5f;
    private int _maxIndexOfQueue = 24;
    private int _rotaryIndex = 10;

    public Vector3[] Positions => _positions.ToArray();

    private void Awake()
    {
        CalculatePositions();
    }

    private void CalculatePositions()
    {
        Vector3 pozition = Vector3.zero;
        int indexOfPosition;

        for (int i = 0; i < _maxIndexOfQueue; i++)
        {
            indexOfPosition = _maxIndexOfQueue - i;

            pozition.z = Mathf.Min(indexOfPosition, _rotaryIndex - 1) * _stepSize;
            pozition.x = Mathf.Max(0, indexOfPosition - _rotaryIndex + 1) * _stepSize;
            pozition.x *= -1;

            _positions.Add(transform.position + pozition);
        }
    }
}
