using UnityEngine;

namespace Scripts.Model.Money
{
    public class Prices : MonoBehaviour
    {
        [field: SerializeField] public int ShufflingBuses { get; private set; }
        [field: SerializeField] public int ArrangingPassengers { get; private set; }
        [field: SerializeField] public int ViewingAd { get; private set; }
    }
}