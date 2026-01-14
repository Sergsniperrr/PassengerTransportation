using System;
using TMPro;
using UnityEngine;

namespace Scripts.Model.Elevators
{
    public class BusCounter : MonoBehaviour
    {
        private TextMeshProUGUI _busCounterText;

        public int Count { get; private set; }

        private void Awake()
        {
            _busCounterText = GetComponentInChildren<TextMeshProUGUI>();

            if (_busCounterText == null)
                throw new NullReferenceException(nameof(_busCounterText));
        }

        public void SetCount(int count)
        {
            Count = count > 0 ? count : throw new ArgumentOutOfRangeException(nameof(count));

            _busCounterText.text = $"{count}";
        }

        public void Decrement()
        {
            if (Count > 0)
                _busCounterText.text = $"{--Count}";
        }
    }
}