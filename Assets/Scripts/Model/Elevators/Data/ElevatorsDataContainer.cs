using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ElevatorsDataContainer
{
    [field : SerializeField] public ElevatorMultipliers Horizontal { get; private set; }
    [field : SerializeField] public ElevatorMultipliers Vertical { get; private set; }
    [field : SerializeField] public ElevatorMultipliers DiagonalLeft { get; private set; }
    [field : SerializeField] public ElevatorMultipliers DiagonalRight { get; private set; }

    public void SetHorizontal(ElevatorMultipliers value) =>
        Horizontal = value;

    public void SetVertical(ElevatorMultipliers value) =>
        Vertical = value;

    public void SetDiagonalLeft(ElevatorMultipliers value) =>
        DiagonalLeft = value;

    public void SetDiagonalRight(ElevatorMultipliers value) =>
        DiagonalRight = value;
}
