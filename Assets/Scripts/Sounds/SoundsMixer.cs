using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

public class SoundsMixer : MonoBehaviour
{
    [SerializeField] private Slider _sliderMusic;
    [SerializeField] private Slider _sliderEffects;
    [SerializeField] private AudioMixerGroup _mixerMusicGroup;
    [SerializeField] private AudioMixerGroup _mixerEffectsGroup;
    [SerializeField] private string _musicVariableName;
    [SerializeField] private string _effectsVariableName;
    [SerializeField] private AudioSource _soundEffectDemo;
    [SerializeField] private AudioSource _initialMusic;

    private const float MinVolume = 0.001f;
    private const float MaxVolume = 1f;
    private const int VolumeMultiplier = 20;

    private void OnEnable()
    {
        _sliderMusic.onValueChanged.AddListener(ChangeMusicVolume);
        _sliderEffects.onValueChanged.AddListener(ChangeEffectsVolume);
    }

    private void Start()
    {
        _sliderMusic.value = PlayerPrefs.GetFloat(_musicVariableName, MaxVolume);
        _sliderEffects.value = PlayerPrefs.GetFloat(_effectsVariableName, MaxVolume);

        StartCoroutine(PlayInitialMusicAfterDelay());
    }

    private void OnDisable()
    {
        _sliderMusic.onValueChanged.RemoveListener(ChangeMusicVolume);
        _sliderEffects.onValueChanged.RemoveListener(ChangeEffectsVolume);
    }

    private void ChangeMusicVolume(float volume) =>
        ChangeValue(_mixerMusicGroup, _musicVariableName, volume);

    private void ChangeEffectsVolume(float volume) =>
        ChangeValue(_mixerEffectsGroup, _effectsVariableName, volume);

    private void ChangeValue(AudioMixerGroup mixerGroup, string mixerVariableName, float volume)
    {
        volume = Mathf.Max(MinVolume, volume);
        mixerGroup.audioMixer.SetFloat(mixerVariableName, Mathf.Log10(volume) * VolumeMultiplier);

        PlayerPrefs.SetFloat(mixerVariableName, volume);
    }

    private IEnumerator PlayInitialMusicAfterDelay()
    {
        WaitForSeconds wait = new(0.1f);

        yield return wait;

        _initialMusic.Play();
    }
}
