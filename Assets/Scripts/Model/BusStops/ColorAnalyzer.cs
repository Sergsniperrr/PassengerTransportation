using System.Collections.Generic;
using System.Linq;
using Scripts.View.Color;
using UnityEngine;

namespace Scripts.Model.BusStops
{
    public class ColorAnalyzer : MonoBehaviour
    {
        private const int FailedIndex = -1;

        private readonly Dictionary<Material, Queue<int>> _allPlaces = new ();

        [SerializeField] private Colors _colors;

        private Queue<int> _freeSpotsIndexes;

        private void Awake()
        {
            for (int i = 0; i < _colors.MaterialsCount; i++)
            {
                _allPlaces.Add(_colors.GetMaterial(i), new Queue<int>());
            }
        }

        public int GetPlatformOfDesiredColor(Material passengerMaterial)
        {
            _freeSpotsIndexes = _allPlaces[passengerMaterial];

            if (_freeSpotsIndexes.Count == 0)
                return FailedIndex;

            return _freeSpotsIndexes.Dequeue();
        }

        public void AdFreePlaces(Material material, int spotIndex, int plasesCount)
        {
            for (int i = 0; i < plasesCount; i++)
            {
                _allPlaces[material].Enqueue(spotIndex);
            }
        }

        public bool CheckDesiredColor(Material passengerColor, Spot[] spots) =>
            spots.Any(spot => spot.BusAtBusStop.Material == passengerColor);

        public Queue<Material> GetAllFreeColors()
        {
            var allColors = _allPlaces.SelectMany(pair => Enumerable.Repeat(pair.Key, pair.Value.Count));

            return new Queue<Material>(allColors);
        }
    }
}