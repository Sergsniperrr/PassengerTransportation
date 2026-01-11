using DG.Tweening;
using System;
using Scripts.View.Menu;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _window;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Background _background;
    [SerializeField] private float _duration = 0.5f;

    public event Action GameStarted;

    private void OnEnable()
    {
        _startGameButton.onClick.AddListener(StartGame);
        _background.gameObject.SetActive(true);
        _background.Show();
    }

    private void StartGame()
    {
        _startGameButton.onClick.RemoveListener(StartGame);

        Close();

        GameStarted?.Invoke();
    }

    private void Close()
    {
        _background.Hide();

        _window.DOFade(0, _duration).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
