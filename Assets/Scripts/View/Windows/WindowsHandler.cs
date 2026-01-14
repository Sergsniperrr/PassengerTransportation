using System;
using UnityEngine;

namespace Scripts.View.Windows
{
    public class WindowsHandler : MonoBehaviour
    {
        [SerializeField] private SimpleWindow _windowBeginLevel;
        [SerializeField] private LevelCompleteWindow _windowLevelComplete;
        [SerializeField] private FailWindows _windowLevelFailed;
        [SerializeField] private WarningWindow _windowWarning;

        public event Action<bool> ResultResieved;

        public SimpleWindow OpenBeginLevel(float delay = 0f) =>
            Open(_windowBeginLevel, delay);

        public SimpleWindow OpenLevelComplete(float delay = 0f) =>
            Open(_windowLevelComplete, delay);

        public void OpenWarningWindow(float delay = 0f)
        {
            WarningWindow warningWindow = (WarningWindow)Open(_windowWarning, delay);

            warningWindow.Closed += TakeResult;
        }

        private SimpleWindow Open(SimpleWindow window, float delay)
        {
            window.gameObject.SetActive(true);
            window.Open(delay);

            return window;
        }

        private void TakeResult(SimpleWindow window)
        {
            window.Closed -= TakeResult;

            WarningWindow warningWindow = (WarningWindow)window;

            if (warningWindow.IsGameContinues == false)
            {
                _windowLevelFailed.gameObject.SetActive(true);
                _windowLevelFailed.Open();
                _windowLevelFailed.Closed += SendFailResult;

                return;
            }

            ResultResieved?.Invoke(true);
        }

        private void SendFailResult(SimpleWindow _)
        {
            _windowLevelFailed.Closed -= SendFailResult;

            ResultResieved?.Invoke(false);
        }
    }
}