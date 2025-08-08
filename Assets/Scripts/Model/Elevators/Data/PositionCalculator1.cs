using UnityEngine;

public class PositionCalculator1
{
    //private Vector3 _zeroPosition;
    private Vector3 _shiftOnX;
    private Vector3 _shiftOnY;

    public ShiftData ShiftData { get; private set; }

    public PositionCalculator1 (Vector3 zeroPosition, Vector3 topRightPosition, Vector3 bottomLeftPosition, Vector2 maxBusShift)
    {
        //_zeroPosition = zeroPosition;

        Vector3 x = (bottomLeftPosition - zeroPosition) / maxBusShift.x;
        Vector3 y = (topRightPosition - zeroPosition) / maxBusShift.y;

        ShiftData = new(x, y, zeroPosition);
    }

    public Vector3 Calculate(Vector2 busCoordinates)
    {
        _shiftOnX = ShiftData.MultiplierOnX * busCoordinates.x;
        _shiftOnY = ShiftData.MultiplierOnY * busCoordinates.y;

        return ShiftData.ZeroPosition + _shiftOnX + _shiftOnY;
    }
}
