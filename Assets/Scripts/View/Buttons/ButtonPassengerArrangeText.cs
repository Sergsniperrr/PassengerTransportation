namespace Scripts.View.Buttons
{
    public class ButtonPassengerArrangeText : FrontButton
    {
        protected override void Awake()
        {
            base.Awake();

            SetText($"{Prices.ArrangingPassengers}");
        }
    }
}