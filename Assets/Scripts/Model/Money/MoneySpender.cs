using System;
using UnityEngine;
using UnityEngine.UI;

public class MoneySpender : MonoBehaviour
{
    public enum PriceNames
    {
        ShufflingBuses,
        ArrangingPassengers
    }

    [SerializeField] private PriceNames _priceName;
    [SerializeField] private Prices _prices;
    [SerializeField] private Button _button;
    [SerializeField] private Vector3 _spentViewPosition;
    [SerializeField] private MoneySpentViewSpawner _spentSpawner;

    public event Action<int> PurchaseCompleted;

    public int Price { get; private set; }

    private void Awake()
    {
        switch (_priceName)
        {
            case PriceNames.ShufflingBuses:
                Price = _prices.ShufflingBuses;
                break;

            case PriceNames.ArrangingPassengers:
                Price = _prices.ArrangingPassengers;
                break;
        }
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
