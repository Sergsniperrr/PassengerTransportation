using Scripts.Model.Other;
using UnityEngine;

public class MoneySpentViewSpawner : Spawner<MoneySpendView>
{
    private MoneySpendView _moneySpent;

    protected override void Awake()
    {
        InitialPosition = transform.position;
        base.Awake();
    }

    public void Spawn(Vector3 position, int price)
    {
        _moneySpent = GetObject();
        _moneySpent.PlayBuyEffect(position, price);
    }
}
