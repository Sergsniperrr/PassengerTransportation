using Scripts.Model.Money;
using TMPro;
using UnityEngine;

namespace Scripts.View.Buttons
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FrontButton : MonoBehaviour
    {
        [SerializeField] protected Prices Prices;

        private TextMeshProUGUI _text;

        protected virtual void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        protected void SetText(string text) =>
            _text.text = text;
    }
}