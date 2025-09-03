using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonDisabler : MonoBehaviour
{
    private Button _button;
    private DarkeningLayer _darkeningLayer;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _darkeningLayer = GetComponentInChildren<DarkeningLayer>();

        if (_darkeningLayer == null)
            throw new NullReferenceException(nameof(_darkeningLayer));
    }

    public void Enable() =>
        SetActive(true);

    public void Disable() =>
        SetActive(false);

    private void SetActive(bool isEnabled)
    {
        _darkeningLayer.gameObject.SetActive(!isEnabled);
        _button.interactable = isEnabled;
    }
}
