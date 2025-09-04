using System;
using UnityEngine;

public class GameButtons : MonoBehaviour
{
    public event Action ButtonsActivated;

    private void OnEnable()
    {
        ButtonsActivated?.Invoke();
    }
}
