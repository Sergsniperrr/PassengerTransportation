using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BusEngineSound : MonoBehaviour
{
    private readonly float _muteSoundsValue = 0f;
    private readonly float _playSoundsValue = 1f;
    private readonly float _randomShiftSize = 0.1f;
    private readonly float _fadeOutSpeed = 4f;

    private AudioSource _sound; 
    private float _initialPich;

    private void Awake()
    {
        _sound = GetComponent<AudioSource>();
        _initialPich = _sound.pitch;
    }

    public void PlaySound()
    {
        _sound.pitch = Random.Range(_initialPich - _randomShiftSize, _initialPich + _randomShiftSize);
        _sound.Play();
        _sound.volume = _playSoundsValue;
    }

    public void StopSound()
    {
        StartCoroutine(SmoothFading());
    }

    public void MoveOut(int busStopIndex) =>
        StartCoroutine(MoveOutAfterDelay(busStopIndex));

    private IEnumerator SmoothFading()
    {
        while (_sound.volume > _muteSoundsValue)
        {
            _sound.volume = Mathf.MoveTowards(_sound.volume, _muteSoundsValue, _fadeOutSpeed * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator MoveOutAfterDelay(int busStopIndex)
    {
        float minDuration = 0.2f;
        float distanceMultiplier = 0.05f;
        WaitForSeconds wait = new(busStopIndex * distanceMultiplier + minDuration);

        PlaySound();

        yield return wait;

        StartCoroutine(SmoothFading());
    }
}
