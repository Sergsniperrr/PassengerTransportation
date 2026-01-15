using System;
using Scripts.View.Coins;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Model.Money
{
    public class MoneySpender : MonoBehaviour
    {
        [SerializeField] private PriceNames _priceName;
        [SerializeField] private Prices _prices;
        [SerializeField] private Button _button;
        [SerializeField] private Vector3 _spentViewPosition;
        [SerializeField] private MoneySpentViewSpawner _spentSpawner;

        public event Action<int> PurchaseCompleted;

        private enum PriceNames
        {
            ShufflingBuses,
            ArrangingPassengers
        }

        public int Price { get; private set; }

        private void Awake()
        {
            Price = _priceName switch
            {
                PriceNames.ShufflingBuses => _prices.ShufflingBuses,
                PriceNames.ArrangingPassengers => _prices.ArrangingPassengers,
                _ => Price
            };
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Buy);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Buy);
        }

        public void Buy()
        {
            _spentSpawner.Spawn(_spentViewPosition, Price);

            PurchaseCompleted?.Invoke(Price);
        }
    }
}