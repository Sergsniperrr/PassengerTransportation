using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ButtonPulsation))]
public class ButtonPassengerArrangeView : MonoBehaviour
{
    private readonly float _duration = 5f;

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

        StartCoroutine(Pulsate());
    }

    private void DisablePulsation()
    {
        _animator.enabled = true;
        _pulsation.enabled = false;
    }

    private IEnumerator Pulsate()
    {
        WaitForSeconds wait = new(_duration);

        yield return wait;

        DisablePulsation();
    }
}
