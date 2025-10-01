using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class InterAdsHandler : MonoBehaviour
{
    [SerializeField] private PreInterWindow _preInterCounter;
    [SerializeField] private Level _level;
    [SerializeField] private int _interval;

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
    }

    private void StartCounting()
    {
        RestartCounting();

        YG2.onCloseAnyAdv += RestartCounting;
    }

    private void RestartCounting()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        yield return _wait;

        _preInterCounter.gameObject.SetActive(true);
        _preInterCounter.StartTimer();
    }

    private void StopCounting()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        if (_preInterCounter.isActiveAndEnabled)
            _preInterCounter.Close();

        YG2.onCloseAnyAdv -= RestartCounting;
    }
}
