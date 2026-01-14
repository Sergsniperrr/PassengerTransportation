using System.Collections.Generic;
using Scripts.Model.Money;
using Scripts.Presenters;
using UnityEngine;

namespace Scripts.View.Buttons
{
    public class ButtonsActivateHandler : MonoBehaviour
    {
        private readonly Dictionary<ButtonDisabler, int> _buttonsPrices = new();

        [SerializeField] private ButtonDisabler _passengersArrangeButton;
        [SerializeField] private ButtonDisabler _busesShuffleButton;
        [SerializeField] private MoneyCounter _wallet;
        [SerializeField] private GameButtons _gameButtons;
        [SerializeField] private BusStop _busStop;

        private ButtonDisabler _buttonPassengerArrange;
        private bool _isAllPlacesVacate = true;

        private void Start()
        {
            if (_passengersArrangeButton.TryGetComponent(out MoneySpender passengerArrange))
            {
                _buttonsPrices.Add(_passengersArrangeButton, passengerArrange.Price);
            }
            else
            {
                throw new MissingComponentException(nameof(MoneySpender));
            }

            if (_busesShuffleButton.TryGetComponent(out MoneySpender busesShuffle))
            {
                _buttonsPrices.Add(_busesShuffleButton, busesShuffle.Price);
            }
            else
            {
                throw new MissingComponentException(nameof(MoneySpender));
            }

            _wallet.ValueChanged += HandleActivateButtons;
            _gameButtons.ButtonsActivated += HandleActivateButtons;
            _busStop.PlacesVacateChanged += ChangePlacesVacateFlag;

            _gameButtons.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _wallet.ValueChanged -= HandleActivateButtons;
            _gameButtons.ButtonsActivated -= HandleActivateButtons;
            _busStop.PlacesVacateChanged -= ChangePlacesVacateFlag;
        }

        private void HandleActivateButtons(int money)
        {
            HandleActivateBusesShuffleButton(money);
            HandleActivatePassengersArrangeButton(money);
        }

        private void HandleActivateButtons()
        {
            HandleActivateButtons(_wallet.Count);
        }

        private void HandleActivateBusesShuffleButton(int money)
        {
            if (CheckForEnoughMoney(money, _busesShuffleButton))
            {
                _busesShuffleButton.Enable();
            }
            else
            {
                _busesShuffleButton.Disable();
            }
        }

        private void HandleActivatePassengersArrangeButton(int money)
        {
            if (_isAllPlacesVacate)
            {
                _passengersArrangeButton.Disable();

                return;
            }

            if (CheckForEnoughMoney(money, _passengersArrangeButton))
            {
                _passengersArrangeButton.Enable();
            }
            else
            {
                _passengersArrangeButton.Disable();
            }
        }

        private bool CheckForEnoughMoney(int money, ButtonDisabler button)
        {
            if (_buttonsPrices.ContainsKey(button) == false)
                throw new KeyNotFoundException(nameof(button));

            bool result = false;

            if (money > 0)
            {
                result = money >= _buttonsPrices[button];
            }

            return result;
        }

        private void ChangePlacesVacateFlag(bool isAllPlacesVacate)
        {
            _isAllPlacesVacate = isAllPlacesVacate;
            HandleActivatePassengersArrangeButton(_wallet.Count);
        }
    }
}