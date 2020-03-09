namespace WpfClient.Events
{
    public class OpenRepairRequestDetailEvent
    {
        public object Sender { get; }

        public int Id { get; }

        public bool IsMyRepairRequest { get; }

        public OpenRepairRequestDetailEvent(object sender, int id, bool isMyRepairRequest)
        {
            Sender = sender;
            Id = id;
            IsMyRepairRequest = isMyRepairRequest;
        }
    }
}
