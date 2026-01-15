using Scripts.Model.Coins;
using YG;

namespace Scripts.Model.Score
{
    public class ScoreCounter : CoinsCounter
    {
        private void Start()
        {
            SetValue(YG2.saves.Score);
        }
    }
}