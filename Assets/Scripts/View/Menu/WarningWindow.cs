using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningWindow : SimpleWindow
{
    [SerializeField] private Button _passengersArrangingButton;
    //[SerializeField] private FadePulsation _busStopContour;

    public bool IsGameContinues { get; private set; }

    public override void Open(float delay = 0)
    {
        base.Open(delay);
        IsGameContinues = false;

        _passengersArrangingButton.onClick.AddListener(ArrangePassengers);
    }

    private void ArrangePassengers()
    {
        _passengersArrangingButton.onClick.RemoveListener(ArrangePassengers);

        IsGameContinues = true;

        Close();
    }
}
