namespace Scripts.View.Buttons
{
    public class ButtonBusShuffleText : FrontButton
    {
        protected override void Awake()
        {
            base.Awake();

            SetText($"{Prices.ShufflingBuses}");
        }
    }
}