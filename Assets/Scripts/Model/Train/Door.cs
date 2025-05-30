using System;
using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    private readonly float _positionXOfOpenState = -8.2f;
    private readonly float _positionXOfCloseState = -6.778f;
    private readonly float _duration = 0.5f;

    public event Action Opened;
    public event Action Closed;

    public void Open()
    {
        transform.DOLocalMoveX(_positionXOfOpenState, _duration).OnComplete(() =>
        {
            Opened?.Invoke();
        });
    }

    public void Close()
    {
        transform.DOLocalMoveX(_positionXOfCloseState, _duration).OnComplete(() =>
        {
            Closed?.Invoke();
        });
    }
}
