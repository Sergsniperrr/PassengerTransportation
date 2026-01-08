using System.Collections;
using System.Collections.Generic;
using Scripts.Presenter;
using UnityEngine;

public class SavesHandler : MonoBehaviour
{
    [SerializeField] private Saver _saver;
    [SerializeField] private Level _level;
    [SerializeField] private float _period = 2f;

    private WaitForSeconds _wait;
    private Coroutine _coroutine;

    private void Awake()
    {
        _wait = new WaitForSeconds(_period);
    }

    private void OnEnable()
    {
        _level.GameActivated += StartTimer;
        _level.AllBussesLeft += StopTimer;
    }

    private void OnDisable()
    {
        _level.GameActivated -= StartTimer;
        _level.AllBussesLeft -= StopTimer;
    }

    private void StartTimer(Queue<Bus> _)
    {
        StopTimer();
        _coroutine = StartCoroutine(SaveAfterDelay());

    }

    private void StopTimer()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator SaveAfterDelay()
    {
        while (enabled)
        {
            yield return _wait;

            _saver.Save();
        }
    }
}
