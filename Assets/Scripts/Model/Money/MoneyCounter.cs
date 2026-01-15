using Scripts.Model.Coins;
using UnityEngine;

namespace Scripts.Model.Money
{
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
}