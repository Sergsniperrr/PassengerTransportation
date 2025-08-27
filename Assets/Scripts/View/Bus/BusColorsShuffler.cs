using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BusColorsShuffler : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Level _level;

    private UndergroundBuses _undergroundBuses;

    public event Action BusesShuffled;

    private void OnEnable()
    {
        _button.onClick.AddListener(ShuffleColors);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ShuffleColors);
    }

    public void InitializeUndergroundBuses(UndergroundBuses buses) =>
        _undergroundBuses = buses != null ? buses : throw new ArgumentNullException(nameof(buses));

    public void ShuffleColors()
    {
        Material material;
        Dictionary<IBusParameters, IBusParameters> duplicates = CreateBusesDuplicates();

        foreach (KeyValuePair<IBusParameters, IBusParameters> duplicate in duplicates)
        {
            material = duplicate.Key.Material;
            duplicate.Key.SetColor(duplicate.Value.Material);
            duplicate.Value.SetColor(material);
        }

        BusesShuffled?.Invoke();
    }

    private Dictionary<IBusParameters, IBusParameters> CreateBusesDuplicates()
    {
        IEnumerable<IBusParameters> enumerableBuses = _level.Buses;
        IEnumerable<IBusParameters> enumerableUndergroundBuses = _undergroundBuses.Buses;

        List<IBusParameters> buses = enumerableBuses.Concat(enumerableUndergroundBuses).ToList();
        Dictionary<IBusParameters, IBusParameters> duplicates = new();

        IBusParameters busKey;
        IBusParameters busValue;
        IBusParameters[] values;

        while (buses.Count > 0)
        {
            busKey = buses[0];
            buses.RemoveAt(0);
            values = buses.Where(bus => bus.SeatsCount == busKey.SeatsCount).ToArray();

            if (values.Length > 0)
            {
                busValue = values[UnityEngine.Random.Range(0, values.Length)];
                buses.Remove(busValue);

                if (duplicates.Keys.Contains(busKey) == false)
                    duplicates.Add(busKey, busValue);
            }
        }

        return duplicates;
    }
}
