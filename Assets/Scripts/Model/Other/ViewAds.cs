using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewAds : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Prices _prices;
    [SerializeField] private MoneyCounter _money;

    private readonly float _transferDuration = 1.5f;

    private void OnEnable()
    {
        _button.onClick.AddListener(TakeReward);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(TakeReward);
    }

    private void TakeReward()
    {
        _money.Add(_prices.ViewingAd, _transferDuration);
    }
}
