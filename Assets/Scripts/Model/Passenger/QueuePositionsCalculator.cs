using System.Collections.Generic;
using UnityEngine;

public class QueuePositionsCalculator : MonoBehaviour
{
    private List<Vector3> _positions = new();

    public int QueueSize { get; } = 25;

    private void Awake()
    {
        CalculatePositions();
    }

    private void CalculatePositions()
    {
        Vector3 pozition = Vector3.zero;
        int rotaryIndex = 10;
        float stepSize = 0.5f;

        for (int i = 0; i < QueueSize; i++)
        {
            pozition.z = Mathf.Min(i, rotaryIndex - 1) * stepSize;
            pozition.x = Mathf.Max(0, i - rotaryIndex + 1) * stepSize;
            pozition.x *= -1;

            _positions.Add(transform.position + pozition);
        }

        _positions.Reverse();
    }
}
