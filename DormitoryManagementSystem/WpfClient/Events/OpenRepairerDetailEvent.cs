namespace WpfClient.Events
{
    public class OpenRepairerDetailEvent
    {
        public object Sender { get; }

        public string Id { get; }

        public OpenRepairerDetailEvent(object sender, string id)
        {
            Sender = sender;
            Id = id;
        }
    }
}
