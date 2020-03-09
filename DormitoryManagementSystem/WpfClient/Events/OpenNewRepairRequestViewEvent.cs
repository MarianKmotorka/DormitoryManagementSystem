namespace WpfClient.Events
{
    public class OpenNewRepairRequestViewEvent
    {
        public object Sender { get; }

        public OpenNewRepairRequestViewEvent(object sender)
        {
            Sender = sender;
        }
    }
}
