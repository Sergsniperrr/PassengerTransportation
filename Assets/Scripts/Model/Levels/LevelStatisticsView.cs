using TMPro;
using UnityEngine;

namespace Scripts.Model.Levels
{
    public class LevelStatisticsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textLevelNumber;
        [SerializeField] private TextMeshProUGUI _textBusesCount;

        public void InitializeData(int levelNumber, int busesCount)
        {
            _textLevelNumber.text = $"{levelNumber}";
            _textBusesCount.text = $"{busesCount}";
        }
    }
}
