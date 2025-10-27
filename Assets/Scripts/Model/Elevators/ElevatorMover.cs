using System;
using DG.Tweening;
using UnityEngine;
using System.Collections;

public class ElevatorMover : MonoBehaviour
{
    private readonly float _liftingDuration = 0.7f;

    private Platform _platform;
    private Vector3 _topPlatformPosition;

    public event Action<Bus> BusLifted;

    private void Awake()
    {
        _platform = GetComponentInChildren<Platform>();

        if (_platform == null)
            throw new ArgumentNullException(nameof(_platform));

        _topPlatformPosition = _platform.transform.localPosition;
    }

    public void LiftBus(Bus bus, float delay = 0f) =>
        StartCoroutine(LiftBusAfterDelay(bus, delay));

    public void LiftBusWithoutDelay(Bus bus)
    {
        if (bus == null)
            throw new ArgumentNullException(nameof(bus));

        Transform busParent = bus.transform.parent;

        bus.transform.SetParent(transform);
        bus.transform.localPosition = _platform.BottomPosition;
        bus.DisableSwingEffect();
        _platform.transform.localPosition = _topPlatformPosition;

        _platform.transform.DOLocalMove(_platform.BottomPosition, _liftingDuration).OnComplete(() =>
        {
            _platform.transform.DOLocalMove(_topPlatformPosition, _liftingDuration);

            bus.gameObject.SetActive(true);
            bus.transform.DOMove(_platform.InitialBusPosition, _liftingDuration).OnComplete(() =>
            {
                bus.transform.SetParent(busParent);
                bus.EnableSwingEffect();

                BusLifted?.Invoke(bus);
            });
        });
    }

    private IEnumerator LiftBusAfterDelay(Bus bus, float delay)
    {
        WaitForSeconds wait = new(delay);

        yield return wait;

        LiftBusWithoutDelay(bus);
    }
}
