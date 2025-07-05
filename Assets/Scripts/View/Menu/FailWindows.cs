using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailWindows : SimpleWindow
{
    [SerializeField] private Music _levelMusic;
    [SerializeField] private Music _failMusic;

    public override void Open(float delay = 0)
    {
        _levelMusic.Stop();
        _failMusic.Play();

        base.Open(delay);
    }
}
