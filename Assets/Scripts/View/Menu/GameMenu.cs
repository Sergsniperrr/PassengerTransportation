using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private Button _soundSettingsButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private Window _soundSettings;
    [SerializeField] private GameResetter _resetter;

    private const string IsRestartPrefName = "IsRestart";

    public event Action<bool> GameActiveChanged;

    private void OnEnable()
    {
        _soundSettings.gameObject.SetActive(true);
        _soundSettingsButton.onClick.AddListener(OpenSoundSettingsMenu);
        _restartButton.onClick.AddListener(Restart);
    }

    private void OnDisable()
    {
        _soundSettingsButton.onClick.RemoveListener(OpenSoundSettingsMenu);
        _restartButton.onClick.RemoveListener(Restart);
    }

    private void Restart()
    {
        _restartButton.onClick.RemoveListener(Restart);

        _resetter.RestartLevel();
    }

    private void OpenSoundSettingsMenu()
    {
        _soundSettings.gameObject.SetActive(true);
        _soundSettings.Open();

        GameActiveChanged?.Invoke(false);

        _soundSettings.Closed += ActivateGame;
    }

    private void ActivateGame()
    {
        _soundSettings.Closed -= ActivateGame;

        GameActiveChanged?.Invoke(true);
    }
}
