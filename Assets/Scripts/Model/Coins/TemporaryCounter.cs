using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TemporaryCounter : MonoBehaviour
{
    private TextMeshProUGUI _text;

    public event Action ResetCompleted;

    public int Value { get; private set; }

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void SetValue(int value)
    {
        Value = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
        _text.text = $"{Value}";
    }

    public void ResetValue(float durationPerOne)
    {
        if (Int32.TryParse(_text.text, out int value) == false)
            value = 0;

        DOTween.To(
           () => value,
           x => value = x,
           0,
           durationPerOne * value)
       .SetEase(Ease.Linear)
       .OnUpdate(() =>
       {
           _text.text = value.ToString();
       })
       .OnComplete(() =>
       {
           ResetCompleted?.Invoke();
       });
    }
}
