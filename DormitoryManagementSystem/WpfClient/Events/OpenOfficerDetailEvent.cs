namespace WpfClient.Events
{
    public class OpenOfficerDetailEvent
    {
        public object Sender { get; }
        public string Id { get; }

        public OpenOfficerDetailEvent(object sender, string id)
        {
            Sender = sender;
            Id = id;
        }
    }
}
