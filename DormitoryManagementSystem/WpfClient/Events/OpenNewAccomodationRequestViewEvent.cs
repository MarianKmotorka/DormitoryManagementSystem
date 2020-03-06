namespace WpfClient.Events
{
    public class OpenNewAccomodationRequestViewEvent
    {
        public object Sender { get; }

        public OpenNewAccomodationRequestViewEvent(object sender)
        {
            Sender = sender;
        }
    }
}
