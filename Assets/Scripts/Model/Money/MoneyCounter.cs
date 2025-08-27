using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCounter : CoinsCounter
{
    [SerializeField] private CoinsOnBusStop _coinsPool;

    private void OnEnable()
    {
        _coinsPool.CoinResieved += AddOneCoin;
    }

    private void OnDisable()
    {
        _coinsPool.CoinResieved -= AddOneCoin;
    }
}
