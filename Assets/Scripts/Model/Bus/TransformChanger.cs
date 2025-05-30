using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(ParticleSystem))]
public class TransformChanger : MonoBehaviour
{
    private const int HalfDevider = 2;

    private Roof _roof;
    private ParticleSystem _smoke;
    private Vector3 _sizeAtStop = new(1.05f, 0.85f, 0.9f);
    private Vector3 _pulsationScale = new(1.2f, 0.5f, 0.9f);
    private float _pulsationDuration = 0.07f;


    private void Awake()
    {
        _smoke = GetComponent<ParticleSystem>();
        _roof = GetComponentInChildren<Roof>();

        if (_roof == null)
            throw new NullReferenceException(nameof(_roof));

    }

    public void GrowToHalfSizeAtStop()
    {
        Vector3 scale = transform.localScale;
        transform.localScale = (_sizeAtStop - scale) / HalfDevider + scale;
    }

    public void GrowToFullSizeAtStop()
    {
        transform.localScale = _sizeAtStop;
    }

    public void DisableRoof() =>
        _roof.gameObject.SetActive(false);

    public void PulsateSize()
    {
        transform.localScale = _pulsationScale;
        transform.DOScale(_sizeAtStop, _pulsationDuration);
    }

    public void EnableSmoke() =>
        _smoke.Play();

    public void DisableSmoke() =>
        _smoke.Stop();
}
