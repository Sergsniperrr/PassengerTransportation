using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelMaker : MonoBehaviour
{
    [SerializeField] private BusSpawner _spawner;
    [SerializeField] private TextMeshProUGUI _textLevelCount;
    [SerializeField] private TMP_InputField _inputLevel;
    [SerializeField] private Button _readButton;
    [SerializeField] private Button _writeButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _loadButton;
    [SerializeField] private TextAsset _jsonResource;

    private const int FailedIndex = -1;

    private LevelsDataContainer _container;

    private void Awake()
    {
        Load();
    }

    private void OnEnable()
    {
        _readButton.onClick.AddListener(Read);
        _writeButton.onClick.AddListener(Write);
        _saveButton.onClick.AddListener(Save);
        _loadButton.onClick.AddListener(Load);
    }

    private void OnDisable()
    {
        _readButton.onClick.RemoveListener(Read);
        _writeButton.onClick.RemoveListener(Write);
        _saveButton.onClick.RemoveListener(Save);
        _loadButton.onClick.RemoveListener(Load);
    }

    private void Load()
    {
        _container = JsonUtility.FromJson<LevelsDataContainer>(_jsonResource.text);

        if (_container == null)
        {
            Debug.Log("File is empty!");
        }
        else
        {
            Debug.Log("File was load successfully!");
            _textLevelCount.text = $"{_container.Count}";
        }
    }

    private void Save()
    {
        LevelDataSaver.Save(_container);
    }

    private void Write()
    {
        int levelNumber = InputLevelNumber();

        if (levelNumber == FailedIndex)
            return;

        Insert(CreateLevel(), levelNumber - 1);

        int count = _container.GetLevel(levelNumber).Buses.Length;
        Debug.Log($"Level {levelNumber} ({count} buses) has been successfully writed!");
    }

    private void Read()
    {
        int levelNumber = InputLevelNumber();

        if (levelNumber == FailedIndex)
            return;

        _spawner.SpawnCreatedLevel(_container.GetLevel(levelNumber).Buses);
    }

    private int InputLevelNumber()
    {
        if (int.TryParse(_inputLevel.text, out int levelNumber) == false)
        {
            Debug.Log("Level number is incorrect!!!");

            return FailedIndex;
        }

        return levelNumber;
    }

    private void Insert(LevelData level, int levelNumber)
    {
        if (levelNumber < 0)
        {
            Debug.Log("Level number is incorrect!!!");
            return;
        }

        if (levelNumber >= _container.Count)
        {
            _container.AddLevel(level);
            return;
        }

        _container.ReplaceLevel(level, levelNumber);
    }

    private LevelData CreateLevel()
    {
        Bus[] buses = _spawner.GetBuses();

        LevelData levelData = new();
        List<BusData> busesData = new();

        foreach (Bus bus in buses)
        {
            BusData busData = new(bus.SeatsCount, bus.transform.position, bus.transform.rotation);

            busesData.Add(busData);
        }

        levelData.AddBusesData(busesData.ToArray());

        return levelData;
    }
}
