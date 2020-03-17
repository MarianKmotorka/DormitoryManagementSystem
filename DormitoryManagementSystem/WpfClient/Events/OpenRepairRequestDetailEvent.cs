namespace WpfClient.Events
{
    public class OpenRepairRequestDetailEvent
    {
        public object Sender { get; }

        public int Id { get; }

        public OpenRepairRequestDetailEvent(object sender, int id)
        {
            Sender = sender;
            Id = id;
        }
    }
}
