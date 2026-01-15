namespace Scripts.View.Buttons
{
    public class ButtonViewAdText : FrontButton
    {
        protected override void Awake()
        {
            base.Awake();

            SetText($"+{Prices.ViewingAd}");
        }
    }
}