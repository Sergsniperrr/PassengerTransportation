using System;
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

    public void Add(int value) =>
        ChangeValue(value);

    public void Remove(int value)
    {
        if (value > Count)
            throw new ArgumentOutOfRangeException(nameof(value));

        ChangeValue(-value, true);
    }

    public void AddOneCoin(bool canShowEffect = false)
    {
        Count++;
        _view.ChangeValue(Count - 1, Count, canShowEffect);

        ValueChanged?.Invoke(Count);
    }

    private void ChangeValue(int value, bool isFastChange = false)
    {
        if (Count + value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        int currentValue = Count;

        if (isFastChange)
        {
            Count += value;
            _view.ChangeValue(currentValue, Count, isFastChange);
            ValueChanged?.Invoke(Count);
        }
        else
        {
            _deferredChange = Count + value;
            _view.ChangeValue(currentValue, _deferredChange, isFastChange);

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
