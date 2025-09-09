using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ButtonPulsation))]
public class ButtonPassengerArrangeView : MonoBehaviour
{
    private Button _button;
    private Animator _animator;
    private ButtonPulsation _pulsation;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _pulsation = GetComponent<ButtonPulsation>();
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        DisablePulsation();

        _button.onClick.AddListener(DisablePulsation);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(DisablePulsation);
    }

    public void EnablePulsation()
    {
        _animator.enabled = false;
        _pulsation.enabled = true;
    }

    private void DisablePulsation()
    {
        _animator.enabled = true;
        _pulsation.enabled = false;
    }
}
