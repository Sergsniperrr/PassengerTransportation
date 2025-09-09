using System;
using UnityEngine;

public class GameButtons : MonoBehaviour
{
    [SerializeField] private ButtonPassengerArrangeView _buttonPassengerArrange;

    public event Action ButtonsActivated;

    private void OnEnable()
    {
        ButtonsActivated?.Invoke();
    }

    public void PlayButtonPassengerArrangePulsation() =>
        _buttonPassengerArrange.EnablePulsation();
}
