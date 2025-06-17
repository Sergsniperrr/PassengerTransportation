using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] private AudioSource _music;

    public void Play()
    {
        _music.Play();
    }

    public void Stop()
    {
        _music.volume = 0f;
    }
}
