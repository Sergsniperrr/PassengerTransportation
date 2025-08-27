using UnityEngine;
using UnityEngine.UI;

public class GameButtons : MonoBehaviour
{
    [SerializeField] private Button _leaderboard;
    [SerializeField] private Button _soundSettings;
    [SerializeField] private Button _gameReset;
    [SerializeField] private Button _passengerShuffle;
    [SerializeField] private Button _busShuffle;
    [SerializeField] private Button _viewAd;

    private void Awake()
    {
        SetActive(false);
    }

    public void SetActive(bool isActive)
    {
        _leaderboard.gameObject.SetActive(isActive);
        _soundSettings.gameObject.SetActive(isActive);
        _gameReset.gameObject.SetActive(isActive);
        _passengerShuffle.gameObject.SetActive(isActive);
        _busShuffle.gameObject.SetActive(isActive);
        _viewAd.gameObject.SetActive(isActive);
    }
}
