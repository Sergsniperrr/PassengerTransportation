using System;
using System.Collections.Generic;
using Scripts.View.Color;
using UnityEngine;

namespace Scripts.Model.Levels
{
    public class UndergroundBuses : MonoBehaviour
    {
        private readonly Queue<BusUnderground> _buses = new ();
        private readonly int[] _seats = { 4, 6, 10 };

        [SerializeField] private Colors _colors;

        private int _seatsCount;

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
            _seatsCount = _seats[UnityEngine.Random.Range(0, _seats.Length)];

            return new BusUnderground(_seatsCount, _colors.GetRandomColor());
        }
    }
}