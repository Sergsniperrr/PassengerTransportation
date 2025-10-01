using System.Collections;
using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PreInterCounter : MonoBehaviour
{
    private readonly WaitForSeconds _waitOneSecond = new(1f);

    private TextMeshProUGUI _text;

    public event Action CountingCompleted;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void StartCounting(int secondsCount)
    {
        StartCoroutine(CountDown(secondsCount));
    }

    private IEnumerator CountDown(int secondsCount)
    {
        for (int i = secondsCount; i > 0; i--)
        {
            _text.text = i.ToString();

            yield return _waitOneSecond;
        }

        CountingCompleted?.Invoke();
    }
}
