using System;
using UnityEngine;
using YG;

[RequireComponent(typeof(CoinsView))]
[RequireComponent(typeof(PlayerStatistics))]
public class CoinsHandler : MonoBehaviour
{
    [SerializeField] private MoneySpender _shuffleBusesBuyer;
    [SerializeField] private MoneySpender _arrangePassengerBuyer;

    [field: SerializeField] public MoneyCounter Money { get; private set; }
    [field: SerializeField] public ScoreCounter Score { get; private set; }

    private PlayerStatistics _statistics;
    private CoinsView _countersView;
    private CoinsRecipient _coinsRecipient;

    public int Level { get; private set; }
    public int TemporaryMoney { get; private set; }
    public int TemporaryScore { get; private set; }
    public int MoneyBuffer { get; private set; }

    private void Awake()
    {
        _countersView = GetComponent<CoinsView>();
        _statistics = GetComponent<PlayerStatistics>();
        _coinsRecipient = GetComponent<CoinsRecipient>();
    }

    private void OnEnable()
    {
        MoneyBuffer = YG2.saves.Money;
    }

    public void ShowCounters() =>
        _countersView.Show();

    public void HideCounters() =>
        _countersView.Hide();

    public void InitializeNewLevel(int level, int busCount)
    {
        if (busCount < 0)
            throw new ArgumentOutOfRangeException(nameof(busCount));

        if (level < 0)
            throw new ArgumentOutOfRangeException(nameof(level));

        Money.SetValue(MoneyBuffer);
        _statistics.StartCollectData(busCount, level);
        Level = level;

        _shuffleBusesBuyer.PurchaseCompleted += Buy;
        _arrangePassengerBuyer.PurchaseCompleted += Buy;
    }

    public void CompleteLevel()
    {
        _shuffleBusesBuyer.PurchaseCompleted -= Buy;
        _arrangePassengerBuyer.PurchaseCompleted -= Buy;

        _statistics.FinishCollectData(Level);

        TemporaryMoney = CoinsCalculator.CalculateMoney(Level, _statistics.BusesCount, _statistics.PlayerSkill);
        TemporaryScore = CoinsCalculator.CalculateScore(_statistics.BusesCount,
                                                         _statistics.MoneySpent,
                                                         _statistics.PlayerSkill);

        _coinsRecipient.TransferCompleted += UpdateMoneyBuffer;
    }

    private void UpdateMoneyBuffer()
    {
        _coinsRecipient.TransferCompleted -= UpdateMoneyBuffer;

        MoneyBuffer = Money.Count;
    }

    private void Buy(int price) =>
        Money.Remove(price);
}
