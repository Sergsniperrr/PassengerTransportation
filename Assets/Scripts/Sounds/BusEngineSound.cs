using System.Collections;
using UnityEngine;

namespace Scripts.Sounds
{
    [RequireComponent(typeof(AudioSource))]
    public class BusEngineSound : MonoBehaviour
    {
        private const float MuteSoundsValue = 0f;
        private const float PlaySoundsValue = 1f;
        private const float RandomShiftSize = 0.1f;
        private const float FadeOutSpeed = 4f;

        private AudioSource _sound;
        private float _initialPitch;

        private void Awake()
        {
            _sound = GetComponent<AudioSource>();
            _initialPitch = _sound.pitch;
        }

        public void PlaySound()
        {
            _sound.pitch = Random.Range(_initialPitch - RandomShiftSize, _initialPitch + RandomShiftSize);
            _sound.Play();
            _sound.volume = PlaySoundsValue;
        }

        public void StopSound()
        {
            StartCoroutine(SmoothFading());
        }

        public void MoveOut(int busStopIndex)
        {
            StartCoroutine(MoveOutAfterDelay(busStopIndex));
        }

        private IEnumerator SmoothFading()
        {
            while (_sound.volume > MuteSoundsValue)
            {
                _sound.volume = Mathf.MoveTowards(
                    _sound.volume,
                    MuteSoundsValue,
                    FadeOutSpeed * Time.deltaTime);

                yield return null;
            }
        }

        private IEnumerator MoveOutAfterDelay(int busStopIndex)
        {
            const float MinDuration = 0.2f;
            const float DistanceMultiplier = 0.05f;

            WaitForSeconds wait = new (busStopIndex * DistanceMultiplier + MinDuration);

            PlaySound();

            yield return wait;

            StartCoroutine(SmoothFading());
        }
    }
}