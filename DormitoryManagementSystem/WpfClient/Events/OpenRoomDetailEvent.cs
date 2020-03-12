namespace WpfClient.Events
{
    public class OpenRoomDetailEvent
    {
        public object Sender { get; }

        public int Id { get; }

        public OpenRoomDetailEvent(object sender, int id)
        {
            Sender = sender;
            Id = id;
        }
    }
}
