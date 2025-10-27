using UnityEngine;
using UnityEngine.UI;
using YG;

public class ViewAds : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Prices _prices;
    [SerializeField] private MoneyCounter _money;

    private const string RewardId = "Money";

    private readonly float _transferDuration = 1.5f;

    private void OnEnable()
    {
        _button.onClick.AddListener(ViewAd);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ViewAd);
    }

    private void ViewAd()
    {
        YG2.RewardedAdvShow(RewardId, TakeReward);
    }

    private void TakeReward()
    {
        _money.Add(_prices.ViewingAd, _transferDuration);
    }
}
