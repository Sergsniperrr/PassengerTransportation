using UnityEngine;

namespace Scripts.Model.Buses
{
    public interface IBusParameters
    {
        Material Material { get; }
        int SeatsCount { get; }

        void SetColor(Material material);
    }
}
