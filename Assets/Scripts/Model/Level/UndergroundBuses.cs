using System;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundBuses : MonoBehaviour
{
    [SerializeField] private Colors _colors;

    private Queue<BusUnderground> _buses = new();
    private int[] _seats = { 4, 6, 10 };

    public int Count => _buses.Count;
    public BusUnderground[] Buses => _buses.ToArray();

    public void Generate(int count)
    {
        for (int i = 0; i < count; i++)
        {
            _buses.Enqueue(GenerateBusData());
        }
    }

    public BusUnderground Dequeue()
    {
        if (_buses.Count == 0)
            throw new Exception("Queue \"Buses\" is empty!");

        return _buses.Dequeue();
    }

    private BusUnderground GenerateBusData()
    {
        return new(_seats[UnityEngine.Random.Range(0, _seats.Length)], _colors.GetRandomColor()); ;
    }
}
