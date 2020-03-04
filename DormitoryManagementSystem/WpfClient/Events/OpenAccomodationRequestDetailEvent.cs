namespace WpfClient.Events
{
    public class OpenAccomodationRequestDetailEvent
    {
        public int Id { get; }

        public object Sender { get; }

        public OpenAccomodationRequestDetailEvent(object sender, int id)
        {
            Id = id;
            Sender = sender;
        }
    }
}
