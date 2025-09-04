using System.Collections.Generic;
using UnityEngine;

public class ButtonsActivateHandler : MonoBehaviour
{
    [SerializeField] private ButtonDisabler[] _buttons;
    [SerializeField] private MoneyCounter _wallet;
    [SerializeField] private GameButtons _gameButtons;

    private Dictionary<ButtonDisabler, int> _buttonsPrices = new();

    private void Start()
    {
        foreach (ButtonDisabler button in _buttons)
        {
            if (button.TryGetComponent(out MoneySpender moneySpender))
                _buttonsPrices.Add(button, moneySpender.Price);
            else
                throw new MissingComponentException(nameof(MoneySpender));
        }

        _wallet.ValueChanged += HandleActivateButtons;
        _gameButtons.ButtonsActivated += HandleActivateButtons;

        _gameButtons.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _wallet.ValueChanged -= HandleActivateButtons;
        _gameButtons.ButtonsActivated -= HandleActivateButtons;
    }

    private void HandleActivateButtons(int money)
    {
        foreach (ButtonDisabler button in _buttons)
        {
            if (CheckForEnoughMoney(money, button))
                button.Enable();
            else
                button.Disable();
        }
    }

    private void HandleActivateButtons()
    {
        HandleActivateButtons(_wallet.Count);
    }

    private bool CheckForEnoughMoney(int money, ButtonDisabler button)
    {
        if (_buttonsPrices.ContainsKey(button) == false)
            throw new KeyNotFoundException(nameof(button));

        bool result = false;

        if (money > 0)
            result = money >= _buttonsPrices[button];

        return result;
    }
}
