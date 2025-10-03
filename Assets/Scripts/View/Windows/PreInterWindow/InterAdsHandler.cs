using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class InterAdsHandler : MonoBehaviour
{
    [SerializeField] private Level _level;
    [SerializeField] private int _minLevelForIntersDisplay;
    [SerializeField] private int _interval;
    [SerializeField] private Button[] _buttons;

    private Coroutine _coroutine;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new(_interval);
    }

    private void OnEnable()
    {
        _level.Started += StartCounting;
        _level.AllBussesLeft += StopCounting;
    }

    private void OnDisable()
    {
        _level.Started -= StartCounting;
        _level.AllBussesLeft -= StopCounting;
        UnsubscribeFromButtons();
    }

    private void StartCounting()
    {
        if (_level.CurrentLevel < _minLevelForIntersDisplay)
            return;

        RestartCounting();

        YG2.onCloseAnyAdv += RestartCounting;
    }

    private void RestartCounting()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        UnsubscribeFromButtons();
        _coroutine = StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        yield return _wait;

        SubscribeToButtons();
    }

    private void StopCounting()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        UnsubscribeFromButtons();
        YG2.onCloseAnyAdv -= RestartCounting;
    }

    private void SubscribeToButtons()
    {
        foreach (Button button in _buttons)
            button.onClick.AddListener(ShowInter);
    }

    private void UnsubscribeFromButtons()
    {
        foreach (Button button in _buttons)
            button.onClick.RemoveListener(ShowInter);
    }

    private void ShowInter()
    {
        UnsubscribeFromButtons();
        YG2.InterstitialAdvShow();
    }
}
