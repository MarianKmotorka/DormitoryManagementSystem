namespace WpfClient.Events
{
    public class OpenRespondToAccomodationRequestViewEvent
    {
        public int RequestId { get; }

        public object Sender { get; }

        public OpenRespondToAccomodationRequestViewEvent(object sender, int requestId)
        {
            Sender = sender;
            RequestId = requestId;
        }
    }
}
