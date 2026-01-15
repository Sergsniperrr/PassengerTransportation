using System;
using Scripts.Model.Other;
using Scripts.View.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Menu
{
    public class GameMenu : MonoBehaviour
    {
        [SerializeField] private Button _soundSettingsButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _leaderboardButton;
        [SerializeField] private Window _soundSettings;
        [SerializeField] private Window _leaderboard;
        [SerializeField] private GameResetter _resetter;

        public event Action<bool> GameActiveChanged;

        private void OnEnable()
        {
            _soundSettings.gameObject.SetActive(true);
            _soundSettingsButton.onClick.AddListener(OpenSoundSettingsMenu);
            _leaderboard.gameObject.SetActive(true);
            _leaderboardButton.onClick.AddListener(OpenLeaderboard);
            _restartButton.onClick.AddListener(Restart);
        }

        private void OnDisable()
        {
            _soundSettingsButton.onClick.RemoveListener(OpenSoundSettingsMenu);
            _leaderboardButton.onClick.RemoveListener(OpenLeaderboard);
            _restartButton.onClick.RemoveListener(Restart);
        }

        private void Restart()
        {
            _restartButton.onClick.RemoveListener(Restart);

            _resetter.RestartLevel();
        }

        private void OpenSoundSettingsMenu() =>
            OpenWindow(_soundSettings);

        private void OpenLeaderboard() =>
            OpenWindow(_leaderboard);

        private void OpenWindow(Window window)
        {
            window.gameObject.SetActive(true);
            window.Open();

            GameActiveChanged?.Invoke(false);

            window.Closed += ActivateGame;
        }

        private void ActivateGame()
        {
            _soundSettings.Closed -= ActivateGame;
            _leaderboard.Closed -= ActivateGame;

            GameActiveChanged?.Invoke(true);
        }
    }
}