using UnityEngine;

public interface IColorGetter
{
    int ColorsCount { get; }
    Material DequeuePassengerColor();
}
