using UnityEngine;

namespace Scripts.Model.Passengers
{
    [RequireComponent(typeof(Animator))]
    public class PassengerAnimator : MonoBehaviour
    {
        private readonly int _offset = Animator.StringToHash(nameof(_offset));
        private readonly int _isMoving = Animator.StringToHash(nameof(_isMoving));

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animator.SetFloat(_offset, Random.value);
        }

        public void Move() =>
            _animator.SetBool(_isMoving, true);

        public void Stop()
        {
            _animator.SetBool(_isMoving, false);
        }
    }
}