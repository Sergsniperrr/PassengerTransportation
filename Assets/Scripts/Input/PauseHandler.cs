using UnityEngine;

namespace Scripts.Input
{
    public class PauseHandler : MonoBehaviour
    {
        private const float PausedTime = 0f;
        private const float NormalTime = 1f;

        private bool _isWindowActive = true;

        private void Start()
        {
            Application.focusChanged += OnWindowFocusChanged;
        }

        private void OnDestroy()
        {
            Application.focusChanged -= OnWindowFocusChanged;
        }

        private void OnWindowFocusChanged(bool isFocused)
        {
            _isWindowActive = isFocused;

            if (_isWindowActive == false)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        private void PauseGame()
        {
            Time.timeScale = PausedTime;
            AudioListener.pause = true;
        }

        private void ResumeGame()
        {
            Time.timeScale = NormalTime;
            AudioListener.pause = false;
        }
    }
}