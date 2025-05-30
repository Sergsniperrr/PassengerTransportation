using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using DG.Tweening;

public class Train : MonoBehaviour
{
    [SerializeField] private Door _door;

    private readonly float _startPosition = 7f;
    private readonly float _stationPosition = -25.9f;
    private readonly float _finalPosition = -82.8f;
    private readonly float _duration = 1.3f;

    public event Action ArrivedAtStation;
    public event Action LeftStation;

    private void OnEnable()
    {
        _door.Opened += FinishMoveToStation;
        _door.Closed += MoveOutFromStation;
    }

    private void OnDisable()
    {
        _door.Opened -= FinishMoveToStation;
        _door.Closed -= MoveOutFromStation;
    }

    public void MoveToStation()
    {
        ReturnToStartPosition();
        transform.DOMoveX(_stationPosition, _duration)
            .SetEase(Ease.OutSine)
            .OnComplete(() =>
        {
            _door.Open();
        });
    }

    public void LeaveStation() =>
        _door.Close();

    public void MoveOutFromStation()
    {
        transform.DOMoveX(_finalPosition, _duration)
            .SetEase(Ease.InSine)
            .OnComplete(() =>
            {
                LeftStation?.Invoke();
            });
    }

    public void ReturnToStartPosition()
    {
        Vector3 position = transform.position;
        position.x = _startPosition;
        transform.position = position;
    }

    private void FinishMoveToStation() =>
        ArrivedAtStation?.Invoke();
}
