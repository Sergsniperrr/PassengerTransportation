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
    [SerializeField] private int _testLevel;
    [SerializeField] private LevelMaker _levelMaker;

    private const string LevelPrefName = "CurrentLevel";
    private const string IsRestartPrefName = "IsRestart";

    private LevelsDataContainer _levelsData;
    private int _currentLevel;

    private void Awake()
    {
        _levelsData = JsonUtility.FromJson<LevelsDataContainer>(_jsonResource.text);
        //PlayerPrefs.SetInt(LevelPrefName, 0); // ÍÓÆÍÎ ÓÄÀËÈÒÜ ÏÅÐÅÄ ÇÀËÈÂÊÎÉ !!!!!!!
        //_currentLevel = PlayerPrefs.GetInt(LevelPrefName, 0);
        _currentLevel = _testLevel;
        _music.Stop();
    }

    private void Start()
    {
        if (_levelMaker.gameObject.activeSelf == false)
          StartGame();
    }

    private void OnDisable()
    {
        _busSpawner.BusUndergroundSpawned -= _level.AddUndergroundBus;
        _level.Completed -= LevelComplete;
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

        LevelBusCalculator levelCalculator = new(_currentLevel);
        BusData[] levelData = _levelsData.GetLevel(levelCalculator.GetSimpleLevel()).Buses;
        _busSpawner.InitializeUndergroundBuses(levelCalculator, levelData);

        _music.Play();
        _level.Begin(_currentLevel, _music, _busSpawner.SpawnLevel(levelData), _busSpawner.UndergroundBuses);
        _level.ChangeGameActivity(false);

        _busSpawner.BusLeftParkingLot += _level.RemoveBus;
        _level.Completed += LevelComplete;
        _busSpawner.BusUndergroundSpawned += _level.AddUndergroundBus;
        _level.GameStarted += StartElevators;
    }

    private void StartElevators()
    {
        _level.GameStarted -= StartElevators;

        _busSpawner.StartElevators();
    }

    private void LevelComplete()
    {
        _level.Completed -= LevelComplete;
        _busSpawner.BusUndergroundSpawned -= _level.AddUndergroundBus;
        _busSpawner.BusLeftParkingLot -= _level.RemoveBus;

        _currentLevel++;

        //if (_currentLevel >= _levelsData.Count)
        //{
        //    DOTween.Clear(true);
        //    _resetter.BackInMainMenu(--_currentLevel);
        //}

        //DOTween.Clear(true);
        PlayerPrefs.SetInt(LevelPrefName, _currentLevel);

        //_resetter.BackInMainMenu(_currentLevel);

        StartLevel();
        //StartGame();
    }
}
