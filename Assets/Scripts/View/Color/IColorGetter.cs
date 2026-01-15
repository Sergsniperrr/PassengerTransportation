using UnityEngine;

namespace Scripts.View.Color
{
    public interface IColorGetter
    {
        int ColorsCount { get; }
        Material DequeuePassengerColor();
    }
}