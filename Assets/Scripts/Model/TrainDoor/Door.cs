using System;
using DG.Tweening;
using UnityEngine;

namespace Scripts.Model.TrainDoor
{
    public class Door : MonoBehaviour
    {
        private const float PositionXOfOpenState = -8.2f;
        private const float PositionXOfCloseState = -6.778f;
        private const float Duration = 0.5f;

        public event Action Opened;
        public event Action Closed;

        public void Open()
        {
            transform.DOLocalMoveX(PositionXOfOpenState, Duration).OnComplete(() => { Opened?.Invoke(); });
        }

        public void Close()
        {
            transform.DOLocalMoveX(PositionXOfCloseState, Duration).OnComplete(() => { Closed?.Invoke(); });
        }
    }
}