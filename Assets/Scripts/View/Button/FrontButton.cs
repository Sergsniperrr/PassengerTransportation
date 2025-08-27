using TMPro;
using UnityEngine;

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
