using System;
using System.IO;
using UnityEngine;

public class CoordinatesCalculator : MonoBehaviour
{
    [SerializeField] private Elevator _elevator;
    [SerializeField] private Bus _bus;

    [SerializeField] private Transform _bottomRightElevator;
    [SerializeField] private Transform _topRightElevator;
    [SerializeField] private Transform _bottomLeftElevator;

    [SerializeField] private Transform _bottomRightBus;
    [SerializeField] private Transform _topRightBus;
    [SerializeField] private Transform _bottomLeftBus;

    private ElevatorBackground _bottomRightBackground;
    private ElevatorBackground _topRightBackground;
    private ElevatorBackground _bottomLeftBackground;

    private Platform _bottomRightPlatform;
    private Platform _topRightPlatform;
    private Platform _bottomLeftPlatform;

    private ElevatorBackground _background;
    private Platform _platform;
    private Vector2 _busShift;

    private PositionCalculator1 _positionShift;
    private PositionCalculator1 _rotationShift;
    private PositionCalculator1 _backroundPosition;
    private PositionCalculator1 _bottomPlatformPosition;

    private void Awake()
    {
        Vector2 maxBusShift = Vector2.zero;

        _bottomRightBackground = _bottomRightElevator.GetComponentInChildren<ElevatorBackground>();
        _topRightBackground = _topRightElevator.GetComponentInChildren<ElevatorBackground>();
        _bottomLeftBackground = _bottomLeftElevator.GetComponentInChildren<ElevatorBackground>();

        _bottomRightPlatform = _bottomRightElevator.GetComponentInChildren<Platform>();
        _topRightPlatform = _topRightElevator.GetComponentInChildren<Platform>();
        _bottomLeftPlatform = _bottomLeftElevator.GetComponentInChildren<Platform>();

        _background = _elevator.GetComponentInChildren<ElevatorBackground>();
        _platform = _elevator.GetComponentInChildren<Platform>();

        if (_background == null)
            throw new NullReferenceException(nameof(_background));

        maxBusShift.x = _bottomLeftBus.position.x - _bottomRightBus.position.x;
        maxBusShift.y = _topRightBus.position.z - _bottomRightBus.position.z;

        _positionShift = new(_bottomRightElevator.localPosition, _topRightElevator.localPosition,
                             _bottomLeftElevator.localPosition, maxBusShift);

        _rotationShift = new(_bottomRightElevator.eulerAngles, _topRightElevator.eulerAngles,
                             _bottomLeftElevator.eulerAngles, maxBusShift);

        _backroundPosition = new(_bottomRightBackground.transform.localPosition, _topRightBackground.transform.localPosition,
                                 _bottomLeftBackground.transform.localPosition, maxBusShift);

        _bottomPlatformPosition = new(_bottomRightPlatform.BottomPosition, _topRightPlatform.BottomPosition,
                                      _bottomLeftPlatform.BottomPosition, maxBusShift);

        ElevatorMultipliers elevatorMultipliers = new();

        elevatorMultipliers.SetPosition(_positionShift.ShiftData);
        elevatorMultipliers.SetRotation(_rotationShift.ShiftData);
        elevatorMultipliers.SetBackgroundPosition(_backroundPosition.ShiftData);
        elevatorMultipliers.SetBottomPlatformPosition(_bottomPlatformPosition.ShiftData);

        //Save(elevatorMultipliers);

        //CalculateElevatorCoordinates(_bus.transform.position);
    }

    private void Update()
    {
        //CalculateElevatorCoordinates(_bus.transform.position);
    }

    public void Save(ElevatorMultipliers elevatorMultipliers)
    {
        string localSavePath = "F:/GitProjects/PassengerTransportation/Assets/Resources";

        string json = JsonUtility.ToJson(elevatorMultipliers, true);
        string path = $"{localSavePath}/{_elevator.Name}.json";
        File.WriteAllText(path, json);

        Debug.Log("Save Complete!!!");
    }

    private void CalculateElevatorCoordinates(Vector3 busPosition)
    {
        _busShift.x = busPosition.x - _bottomRightBus.position.x;
        _busShift.y = busPosition.z - _bottomRightBus.position.z;

        _elevator.transform.localPosition = _positionShift.Calculate(_busShift);
        _elevator.transform.eulerAngles = _rotationShift.Calculate(_busShift);
        _background.transform.localPosition = _backroundPosition.Calculate(_busShift);
        _platform.SetBottomPosition(_bottomPlatformPosition.Calculate(_busShift));
    }
}
