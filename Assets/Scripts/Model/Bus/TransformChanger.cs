using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformChanger : MonoBehaviour
{
    private const int HalfDevider = 2;

    private Roof _roof;
    private Vector3 _sizeAtStop = new(1.05f, 0.85f, 0.9f);

    private void Awake()
    {
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
}
