using System;
using Scripts.Model.Coins;
using UnityEngine;

[RequireComponent(typeof(CoinsCounterView))]
public class CoinsCounter : MonoBehaviour
{
    private CoinsCounterView _view;
    private int _deferredChange;

    public event Action<int> ValueChanged;

    public int Count { get; private set; }

    private void Awake()
    {
        _view = GetComponent<CoinsCounterView>();
    }

    public void SetValue(int value)
    {
        Count = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
        _view.SetValue(value);

        ValueChanged?.Invoke(Count);
    }

    public void Add(int value, float duration = 0f) =>
        ChangeValue(value, duration);

    public void Remove(int value)
    {
        if (value > Count)
            throw new ArgumentOutOfRangeException(nameof(value));

        ChangeValue(-value);
    }

    public void AddOneCoin()
    {
        Count++;
        _view.ChangeValue(Count - 1, Count);

        ValueChanged?.Invoke(Count);
    }

    private void ChangeValue(int value, float duration = 0f)
    {
        if (Count + value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        int currentValue = Count;

        if (duration == 0f)
        {
            Count += value;
            _view.ChangeValue(currentValue, Count);
            ValueChanged?.Invoke(Count);
        }
        else
        {
            _deferredChange = Count + value;
            _view.ChangeValue(currentValue, _deferredChange, duration);

            _view.ChangeCompleted += ChangeValueDeferred;
        }
    }

    private void ChangeValueDeferred()
    {
        _view.ChangeCompleted -= ChangeValueDeferred;

        Count = _deferredChange;
        ValueChanged?.Invoke(Count);
    }
}
