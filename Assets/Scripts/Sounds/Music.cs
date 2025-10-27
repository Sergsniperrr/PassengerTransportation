using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource _music;

    private const int MinVolume = 0;
    private const int MaxVolume = 1;

    private float _duration;

    public void Play()
    {
        _music.volume = MaxVolume;
        _music.Play();
    }

    public void Stop()
    {
        _music.volume = MinVolume;
    }
}
