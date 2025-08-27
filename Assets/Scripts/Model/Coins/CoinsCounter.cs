using System;
using UnityEngine;

[RequireComponent(typeof(CoinsCounterView))]
public class CoinsCounter : MonoBehaviour
{
    private CoinsCounterView _view;

    public int Coins { get; private set; }

    private void Awake()
    {
        _view = GetComponent<CoinsCounterView>();
    }

    public void SetValue(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        _view.SetValue(value);
    }

    public void Add(int value) =>
        ChangeValue(value);

    public void Remove(int value)
    {
        if (value > Coins)
            throw new ArgumentOutOfRangeException(nameof(value));

        ChangeValue(-value);
    }

    public void AddOneCoin(bool canShowEffect = false)
    {
        Coins++;

        _view.ChangeValue(Coins - 1, Coins, canShowEffect);
    }

    private void ChangeValue(int value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value));

        int currentValue = Coins;
        Coins += value;

        _view.ChangeValue(currentValue, Coins, false);
    }
}
