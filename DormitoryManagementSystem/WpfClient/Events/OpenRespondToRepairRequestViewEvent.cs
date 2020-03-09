namespace WpfClient.Events
{
    public class OpenRespondToRepairRequestViewEvent
    {
        public object Sender { get; }

        public int RequestId { get; }

        public OpenRespondToRepairRequestViewEvent(object sender, int requestId)
        {
            Sender = sender;
            RequestId = requestId;
        }
    }
}
