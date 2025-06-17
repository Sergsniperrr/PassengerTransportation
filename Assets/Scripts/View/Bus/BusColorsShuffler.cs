using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BusColorsShuffler : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Level _level;

    private void OnEnable()
    {
        _button.onClick.AddListener(ShuffleColors);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(ShuffleColors);
    }

    public void ShuffleColors()
    {
        Material material;
        Dictionary<Bus, Bus> duplicates = CreateBusesDuplicates();

        foreach (KeyValuePair<Bus,Bus> duplicate in duplicates)
        {
            material = duplicate.Key.Material;
            duplicate.Key.SetColor(duplicate.Value.Material);
            duplicate.Value.SetColor(material);
        }
    }

    private Dictionary<Bus, Bus> CreateBusesDuplicates()
    {
        List<Bus> buses = _level.Buses;
        Dictionary<Bus, Bus> duplicates = new();
        Bus busKey;
        Bus busValue;
        Bus[] values;

        while (buses.Count > 0)
        {
            busKey = buses[0];
            buses.RemoveAt(0);
            values = buses.Where(bus => bus.SeatsCount == busKey.SeatsCount).ToArray();

            if (values.Length > 0)
            {
                busValue = values[UnityEngine.Random.Range(0, values.Length)];
                buses.Remove(busValue);
                duplicates.Add(busKey, busValue);
            }
        }


        return duplicates;
    }
}
