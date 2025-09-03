using System;
using UnityEngine;

public class GameButtons : MonoBehaviour
{
    public event Action ButtonsActivated;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ButtonsActivated?.Invoke();
    }
}
