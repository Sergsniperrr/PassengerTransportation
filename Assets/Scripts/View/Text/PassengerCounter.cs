using System;
using TMPro;
using UnityEngine;

namespace Scripts.View.Text
{
    public class PassengerCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private int _count;

        public void SetValue(int value)
        {
            _count = value >= 0 ? value : throw new ArgumentOutOfRangeException(nameof(value));
            _text.text = _count.ToString();
        }

        public void DecrementValue() =>
            _text.text = $"{--_count}";
    }
}