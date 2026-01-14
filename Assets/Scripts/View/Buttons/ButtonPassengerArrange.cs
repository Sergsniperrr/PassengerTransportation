using Scripts.View.Color;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PassengerColorArranger))]
public class ButtonPassengerArrange : MonoBehaviour
{
    [SerializeField] private Button _button;

    private PassengerColorArranger _colorArranger;

    private void Awake()
    {
        _colorArranger = GetComponent<PassengerColorArranger>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(_colorArranger.ArrangeColors);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(_colorArranger.ArrangeColors);
    }
}
