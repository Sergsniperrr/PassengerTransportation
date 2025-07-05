using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsHandler : MonoBehaviour
{
    [SerializeField] private Level _level;
    [SerializeField] private BusSpawner _busSpawner;
    [SerializeField] private StartMenu _startMenu;
    [SerializeField] private Music _music;
    [SerializeField] private TextAsset _jsonResource;
    [SerializeField] private GameResetter _resetter;

    private const string LevelPrefName = "CurrentLevel";
    private const string IsRestartPrefName = "IsRestart";

    private LevelsDataContainer _levelsData;
    private int _currentLevel;

    private void Awake()
    {
        _levelsData = JsonUtility.FromJson<LevelsDataContainer>(_jsonResource.text);
        //PlayerPrefs.SetInt(LevelPrefName, 0);
        _currentLevel = PlayerPrefs.GetInt(LevelPrefName, 0);
        _music.Stop();
    }

    private void Start()
    {
        StartGame();
    }

    private void StartGame()
    {
        bool isRestart = Convert.ToBoolean(PlayerPrefs.GetInt(IsRestartPrefName, 0));

        if (isRestart == false)
        {
            _startMenu.gameObject.SetActive(true);
            _startMenu.GameStarted += StartLevel;
        }
        else
        {
            StartCoroutine(StartLevelAfterDelay());
        }
    }

    private IEnumerator StartLevelAfterDelay()
    {
        WaitForSeconds wait = new(0.2f);

        yield return wait;

        StartLevel();
        PlayerPrefs.SetInt(IsRestartPrefName, Convert.ToInt32(false));
    }

    private void StartLevel()
    {
        _startMenu.GameStarted -= StartLevel;

        BusData[] levelData = _levelsData.GetLevel(_currentLevel).Buses;

        _music.Play();
        _level.Begin(_currentLevel, _music, _busSpawner.SpawnLevel(levelData));
        _level.ChangeGameActivity(false);
        _level.Completed += LevelComplete;
    }

    private void LevelComplete()
    {
        _level.Completed -= LevelComplete;

        _currentLevel++;

        if (_currentLevel >= _levelsData.Count)
        {
            DOTween.Clear(true);
            _resetter.BackInMainMenu(--_currentLevel);
        }

        PlayerPrefs.SetInt(LevelPrefName, _currentLevel);

        StartLevel();
    }
}
