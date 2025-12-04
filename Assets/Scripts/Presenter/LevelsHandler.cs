using System;
using System.Collections;
using UnityEngine;
using YG;

public class LevelsHandler : MonoBehaviour
{
    [SerializeField] private Level _level;
    [SerializeField] private TutorialFinger _tutorialFinger;
    [SerializeField] private BusSpawner _busSpawner;
    [SerializeField] private StartMenu _startMenu;
    [SerializeField] private Music _music;
    [SerializeField] private TextAsset _jsonResource;
    [SerializeField] private GameResetter _resetter;
    [SerializeField] private LevelMaker _levelMaker;
    [SerializeField] private PlayerSaver _saver;
    [SerializeField] private int _minLevelForViewInterstitial = 5;

    private const string IsRestartPrefName = "IsRestart";
    private const string LeaderboardName = "BestCarriers";
    private const int FirstLevel = 1;

    private LevelsDataContainer _levelsData;
    private int _currentLevel;

    private void Awake()
    {
        _levelsData = DataLoader.GetLevelData(_jsonResource);
        _music.Stop();
    }

    private void Start()
    {
        _currentLevel = YG2.saves.Level;

        if (_currentLevel == FirstLevel)
        {
            _tutorialFinger.gameObject.SetActive(true);
            _tutorialFinger.Enable();
        }

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
        _level.Started += StartElevators;
    }

    private void StartElevators()
    {
        _level.Started -= StartElevators;

        _busSpawner.StartElevators();
    }

    private void LevelComplete()
    {
        _level.Completed -= LevelComplete;
        _busSpawner.BusUndergroundSpawned -= _level.AddUndergroundBus;
        _busSpawner.BusLeftParkingLot -= _level.RemoveBus;

        _currentLevel++;

        _saver.Save();
        YG2.SaveProgress();
        YG2.SetLeaderboard(LeaderboardName, _level.Coins.Score.Count);

        if (_currentLevel >= _minLevelForViewInterstitial)
            YG2.InterstitialAdvShow();

        StartLevel();
    }
}
