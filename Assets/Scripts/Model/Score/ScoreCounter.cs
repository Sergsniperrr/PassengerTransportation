using YG;

public class ScoreCounter : CoinsCounter
{
    private void Start()
    {
        SetValue(YG2.saves.Score);
    }
}
