using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.View.Buttons
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(ButtonPulsation))]
    public class ButtonPassengerArrangeView : MonoBehaviour
    {
        private const float Duration = 5f;

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
            WaitForSeconds wait = new (Duration);

            yield return wait;

            DisablePulsation();
        }
    }
}