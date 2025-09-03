using System;
using UnityEngine;

[RequireComponent(typeof(CoinsCounterView))]
public class CoinsCounter : MonoBehaviour
{
    private CoinsCounterView _view;
    private int _deferredChange;

    public event Action<int> ValueChanged;

    public int Coins { get; private set; }

    private void Awake()
    {
        _view = GetComponent<CoinsCounterView>();
    }

    public void SetValue(int value)
    {
        Coins = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
        _view.SetValue(value);

        ValueChanged?.Invoke(Coins);
    }

    public void Add(int value) =>
        ChangeValue(value);

    public void Remove(int value)
    {
        if (value > Coins)
            throw new ArgumentOutOfRangeException(nameof(value));

        ChangeValue(-value, true);
    }

    public void AddOneCoin(bool canShowEffect = false)
    {
        Coins++;
        _view.ChangeValue(Coins - 1, Coins, canShowEffect);

        ValueChanged?.Invoke(Coins);
    }

    private void ChangeValue(int value, bool isFastChange = false)
    {
        if (Coins + value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        int currentValue = Coins;

        if (isFastChange)
        {
            Coins += value;
            _view.ChangeValue(currentValue, Coins, isFastChange);
            ValueChanged?.Invoke(Coins);
        }
        else
        {
            _deferredChange = Coins + value;
            _view.ChangeValue(currentValue, _deferredChange, isFastChange);

            _view.ChangeCompleted += ChangeValueDeferred;
        }


    }

    private void ChangeValueDeferred()
    {
        _view.ChangeCompleted -= ChangeValueDeferred;

        Coins = _deferredChange;
        ValueChanged?.Invoke(Coins);
    }
}
